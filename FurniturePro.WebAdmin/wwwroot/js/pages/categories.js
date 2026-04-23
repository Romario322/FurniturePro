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
    const closeImportModalBtn = document.getElementById('closeImportModalBtn');
    const cancelImportBtn = document.getElementById('cancelImportBtn');
    const importForm = document.getElementById('importForm');
    const importFileInput = document.getElementById('importFileInput');

    if (openImportModalBtn) {
        openImportModalBtn.addEventListener('click', () => {
            importForm.reset();
            showModal(importModal);
        });
    }

    // Закрытие модалки
    const closeModal = () => hideModal(importModal);
    if (closeImportModalBtn) closeImportModalBtn.addEventListener('click', closeModal);
    if (cancelImportBtn) cancelImportBtn.addEventListener('click', closeModal);

    // Обработка отправки формы
    if (importForm) {
        importForm.addEventListener('submit', async (e) => {
            e.preventDefault();

            const file = importFileInput.files[0];

            if (!file) {
                alert("Выберите файл!");
                return;
            }

            // Собираем данные в FormData (стандартный способ передачи файлов)
            const formData = new FormData();
            formData.append('excelFile', file); // Имя 'excelFile' должно совпадать с параметром в C#

            // Достаем CSRF токен со страницы
            const token = document.querySelector('input[name="__RequestVerificationToken"]').value;

            try {
                closeModal();
                toggleLoader(true);

                // Отправляем POST запрос к обработчику Import на этой же странице
                const response = await fetch('?handler=Import', {
                    method: 'POST',
                    body: formData,
                    headers: {
                        'RequestVerificationToken': token // Передаем токен
                    }
                });

                if (response.ok) {
                    alert('Файл успешно загружен и обработан!');
                    location.reload(); // Перезагружаем страницу для обновления таблицы
                } else {
                    alert('Ошибка при импорте');
                }
            } catch (error) {
                console.error('Ошибка:', error);
            } finally {
                toggleLoader(false);
            }
        });
    }
});