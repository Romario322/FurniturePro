async function syncTable(tableName) {
    let date = await getLastSyncedAt(tableName);
    date = date ? normalizeIsoFraction(date) : '1000-01-01T00:00:00.000000';
    const params = `/API/${tableName}?dateTime=${date}`;
    const resp = await fetch(params);
    const data = await resp.json();

    const hasChanged =
        (data.items && data.items.length > 0) ||
        (data.deletedItems && data.deletedItems.length > 0);

    if (!hasChanged) {
        return;
    }

    await upsertMany(tableName, data.items);
    await deleteMany(tableName, data.deletedItems);
    await setLastSyncedAt(tableName, data.syncDate);
}

function normalizeIsoFraction(s) {
    const withoutZ = s.endsWith('Z') ? s.slice(0, -1) : s;

    const [left, frac = ''] = withoutZ.split('.');
    const frac6 = (frac + '000000').slice(0, 6);

    return `${left}.${frac6}`;
}