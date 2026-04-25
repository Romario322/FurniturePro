import { DictionaryManager } from '../classes/DictionaryManager.js';
import { showModal, hideModal } from '../ui/modal.js';
import { toggleLoader } from '../ui/loader.js';
import { getAll } from '../db/repository.js';

document.addEventListener('DOMContentLoaded', () => {
    // Инициализация менеджера справочников для сущности Materials
    new DictionaryManager(
        'Materials',          // Имя таблицы в IndexedDB (и endpoint API)
        'Material',           // Суффикс для Razor handlers (CreateMaterial и тд)
        'materialsTable',     // HTML ID таблицы
        'createMaterialButton',// HTML ID кнопки создания
        'materialsCount'      // HTML ID счетчика
    );

    const importModal = document.getElementById('importModal');
    const openImportModalBtn = document.getElementById('openImportModalBtn');
    const importForm = document.getElementById('importForm');
    const importFileInput = document.getElementById('importFileInput');
    const fileNameDisplay = document.getElementById('fileNameDisplay');
    const errorModal = document.getElementById('errorModal');
    const errorList = document.getElementById('errorList');

    // Функция для отображения ошибок в модальном окне
    function displayErrors(messages) {
        if (errorList && errorModal) {
            errorList.innerHTML = '';
            messages.forEach(msg => {
                const li = document.createElement('li');
                li.textContent = msg;
                errorList.appendChild(li);
            });
            showModal(errorModal);
        } else {
            alert(messages.join('\n'));
        }
    }

    // Отображение имени файла при выборе
    if (importFileInput && fileNameDisplay) {
        importFileInput.addEventListener('change', function () {
            if (this.files && this.files.length > 0) {
                fileNameDisplay.textContent = '📄 ' + this.files[0].name;
                fileNameDisplay.style.display = 'block';
            } else {
                fileNameDisplay.style.display = 'none';
                fileNameDisplay.textContent = '';
            }
        });
    }

    // Открытие модального окна импорта и сброс предыдущего выбора
    if (openImportModalBtn) {
        openImportModalBtn.addEventListener('click', () => {
            if (importForm) {
                importForm.reset();
                if (fileNameDisplay) {
                    fileNameDisplay.style.display = 'none';
                    fileNameDisplay.textContent = '';
                }
            }
            showModal(importModal);
        });
    }

    // Обработка отправки формы импорта
    if (importForm) {
        importForm.addEventListener('submit', async (e) => {
            e.preventDefault();

            const file = importFileInput.files[0];

            if (!file) {
                displayErrors(["Выберите файл для загрузки!"]);
                return;
            }

            const token = document.querySelector('input[name="__RequestVerificationToken"]')?.value;

            if (!token) {
                displayErrors(["Токен безопасности не найден!"]);
                return;
            }

            try {
                toggleLoader(true);

                // Получаем все существующие материалы из кэша для проверки дубликатов на сервере
                const cachedMaterials = await getAll('Materials');
                const existingNames = cachedMaterials.map(m => m.name);

                const formData = new FormData();
                formData.append("excelFile", file);
                formData.append("existingNamesStr", JSON.stringify(existingNames));

                // Отправляем запрос
                const response = await fetch(`?handler=Import`, {
                    method: 'POST',
                    body: formData,
                    headers: {
                        'RequestVerificationToken': token
                    }
                });

                let result;
                try {
                    result = await response.json();
                } catch (parseError) {
                    displayErrors(["Не удалось обработать ответ от сервера."]);
                    return;
                }

                if (response.ok) {
                    if (result.success) {
                        hideModal(importModal);
                        // Перезагружаем страницу, чтобы таблица и кэш обновились
                        location.reload();
                    } else {
                        displayErrors([result.message || "Произошла ошибка при обработке файла."]);
                    }
                } else {
                    let errorMessages = ['Произошла ошибка при импорте.'];

                    if (result.errors) {
                        errorMessages = Object.values(result.errors).flat();
                    } else if (result.title) {
                        errorMessages = [result.title];
                    } else if (result.message) {
                        errorMessages = [result.message];
                    }

                    displayErrors(errorMessages);
                }
            } catch (error) {
                console.error('Ошибка:', error);
                displayErrors(['Произошла непредвиденная ошибка сети. Проверьте подключение.']);
            } finally {
                toggleLoader(false);
            }
        });
    }
});