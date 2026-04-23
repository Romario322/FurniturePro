export function initPhoneMask(input) {
    if (!input) return;

    const setCursorPosition = (pos, elem) => {
        elem.focus();
        if (elem.setSelectionRange) elem.setSelectionRange(pos, pos);
        else if (elem.createTextRange) {
            const range = elem.createTextRange();
            range.collapse(true);
            range.moveEnd("character", pos);
            range.moveStart("character", pos);
            range.select();
        }
    };

    const mask = (event) => {
        const matrix = "+7 (___) ___-__-__";
        let i = 0;
        let def = matrix.replace(/\D/g, "");
        let val = input.value.replace(/\D/g, "");

        if (def.length >= val.length) val = def;
        if (val[0] === '8') val = '7' + val.substring(1);

        input.value = matrix.replace(/./g, function (a) {
            return /[_\d]/.test(a) && i < val.length ? val.charAt(i++) : i >= val.length ? "" : a;
        });

        if (event.type === "blur") {
            if (input.value.length === 2) input.value = "";
        } else {
            setCursorPosition(input.value.length, input);
        }
    };

    input.addEventListener("input", mask, false);
    input.addEventListener("focus", mask, false);
    input.addEventListener("blur", mask, false);
}