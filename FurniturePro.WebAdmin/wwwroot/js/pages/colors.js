import { DictionaryManager } from '../classes/DictionaryManager.js';
import { showModal, hideModal } from '../ui/modal.js';
import { toggleLoader } from '../ui/loader.js';
import { getAll } from '../db/repository.js';

document.addEventListener('DOMContentLoaded', () => {
    new DictionaryManager(
        'Colors',
        'Color',
        'colorsTable',
        'createColorButton',
        'colorsCount'
    );

    const importModal = document.getElementById('importModal');
    const openImportModalBtn = document.getElementById('openImportModalBtn');
    const importForm = document.getElementById('importForm');
    const importFileInput = document.getElementById('importFileInput');
    const fileNameDisplay = document.getElementById('fileNameDisplay');
    const errorModal = document.getElementById('errorModal');
    const errorList = document.getElementById('errorList');

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
            if (importForm && fileNameDisplay) {
                fileNameDisplay.style.display = 'none';
                fileNameDisplay.textContent = '';
            }
            showModal(importModal);
        });
    }

    if (importForm) {
        importForm.addEventListener('submit', async (e) => {
            e.preventDefault();
            const file = importFileInput.files[0];

            if (!file) {
                displayErrors(["Выберите файл для загрузки!"]);
                return;
            }

            const token = document.querySelector('input[name="__RequestVerificationToken"]').value;

            try {
                toggleLoader(true);
                const cachedColors = await getAll('Colors');
                const existingNames = cachedColors.map(c => c.name);

                const formData = new FormData();
                formData.append("excelFile", file);
                formData.append("existingNamesStr", JSON.stringify(existingNames));

                const response = await fetch(`?handler=Import`, {
                    method: 'POST',
                    body: formData,
                    headers: { 'RequestVerificationToken': token }
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
                        location.reload();
                    } else {
                        displayErrors([result.message || "Произошла ошибка при обработке файла."]);
                    }
                } else {
                    let errorMessages = ['Произошла ошибка при импорте.'];
                    if (result.errors) errorMessages = Object.values(result.errors).flat();
                    else if (result.title) errorMessages = [result.title];
                    else if (result.message) errorMessages = [result.message];

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