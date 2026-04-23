export function normalizeIsoFraction(s) {
    const withoutZ = s.endsWith('Z') ? s.slice(0, -1) : s;

    const [left, frac = ''] = withoutZ.split('.');
    const frac6 = (frac + '000000').slice(0, 6);

    return `${left}.${frac6}`;
}