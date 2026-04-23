import { DictionaryManager } from '../classes/DictionaryManager.js';
import { showModal, hideModal } from '../ui/modal.js';
import { toggleLoader } from '../ui/loader.js';

document.addEventListener('DOMContentLoaded', () => {
    new DictionaryManager(
        'Categories',          // Имя таблицы в IndexedDB (и endpoint API)
        'Category',            // Суффикс для Razor handlers (CreateCategory и тд)
        'categoriesTable',     // HTML ID таблицы
        'createCategoryButton',// HTML ID кнопки создания
        'categoriesCount'      // HTML ID счетчика
    );

    const importModal = document.getElementById('importModal');
    const openImportModalBtn = document.getElementById('openImportModalBtn');
    const importForm = document.getElementById('importForm');
    const importFileInput = document.getElementById('importFileInput');
    const fileNameDisplay = document.getElementById('fileNameDisplay'); // Элемент для имени файла
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

    if (openImportModalBtn) {
        openImportModalBtn.addEventListener('click', () => {
            if (importForm) {
                importForm.reset();
                // Скрываем имя файла при новом открытии модалки
                if (fileNameDisplay) {
                    fileNameDisplay.style.display = 'none';
                    fileNameDisplay.textContent = '';
                }
            }
            showModal(importModal);
        });
    }

    // Обработка отправки формы
    if (importForm) {
        importForm.addEventListener('submit', async (e) => {
            e.preventDefault();

            const file = importFileInput.files[0];

            if (!file) {
                displayErrors(["Выберите файл для загрузки!"]);
                return;
            }

            const formData = new FormData();
            formData.append('excelFile', file);

            const token = document.querySelector('input[name="__RequestVerificationToken"]').value;

            try {
                toggleLoader(true);

                const response = await fetch('?handler=Import', {
                    method: 'POST',
                    body: formData,
                    headers: {
                        'RequestVerificationToken': token
                    }
                });

                if (response.ok) {
                    location.reload();
                } else {
                    let errorMessages = ['Произошла ошибка при импорте.'];
                    try {
                        const errorData = await response.json();
                        if (errorData.errors) {
                            errorMessages = Object.values(errorData.errors).flat();
                        } else if (errorData.title) {
                            errorMessages = [errorData.title];
                        } else if (errorData.message) {
                            errorMessages = [errorData.message];
                        }
                    } catch (parseError) {
                    }
                    displayErrors(errorMessages);
                }
            } catch (error) {
                console.error('Ошибка:', error);
                displayErrors(['Произошла непредвиденная ошибка сети или сервера.']);
            } finally {
                toggleLoader(false);
            }
        });
    }
});