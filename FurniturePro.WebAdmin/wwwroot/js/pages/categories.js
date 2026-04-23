import { DictionaryManager } from '../classes/DictionaryManager.js';

document.addEventListener('DOMContentLoaded', () => {
    new DictionaryManager(
        'Categories',          // Имя таблицы в IndexedDB (и endpoint API)
        'Category',            // Суффикс для Razor handlers (CreateCategory и тд)
        'categoriesTable',     // HTML ID таблицы
        'createCategoryButton',// HTML ID кнопки создания
        'categoriesCount'      // HTML ID счетчика
    );
});

document.getElementById('uploadBtn').addEventListener('click', async function () {
    const fileInput = document.getElementById('excelFileInput');
    const file = fileInput.files[0];

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
    }
});