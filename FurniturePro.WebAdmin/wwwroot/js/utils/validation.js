export function initNumberInputValidation() {
    // 1. Блокировка клавиши "минус" при вводе
    document.body.addEventListener('keydown', function (e) {
        if (e.target.tagName === 'INPUT' && e.target.type === 'number') {
            if (e.key === '-' || e.key === 'Subtract') {
                e.preventDefault();
            }
        }
    });

    // 2. Блокировка вставки текста с минусом (Ctrl+V)
    document.body.addEventListener('paste', function (e) {
        if (e.target.tagName === 'INPUT' && e.target.type === 'number') {
            let clipboardData = (e.clipboardData || window.clipboardData).getData('text');
            if (clipboardData.includes('-')) {
                e.preventDefault();
                let cleanData = clipboardData.replace(/-/g, '');
                if (cleanData) {
                    document.execCommand('insertText', false, cleanData);
                }
            }
        }
    });
}