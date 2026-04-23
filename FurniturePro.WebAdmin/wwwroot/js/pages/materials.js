import { DictionaryManager } from '../classes/DictionaryManager.js';

document.addEventListener('DOMContentLoaded', () => {
    new DictionaryManager(
        'Materials',
        'Material',
        'materialsTable',
        'createMaterialButton',
        'materialsCount'
    );
});