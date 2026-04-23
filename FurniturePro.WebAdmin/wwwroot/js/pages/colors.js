import { DictionaryManager } from '../classes/DictionaryManager.js';

document.addEventListener('DOMContentLoaded', () => {
    new DictionaryManager(
        'Colors',
        'Color',
        'colorsTable',
        'createColorButton',
        'colorsCount'
    );
});