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