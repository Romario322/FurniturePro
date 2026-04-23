import { openDb } from './database.js';

export async function getLastSyncedAt(entityName) {
    const db = await openDb();
    return new Promise((resolve, reject) => {
        const tx = db.transaction('UpdateDates', 'readonly');
        const store = tx.objectStore('UpdateDates');
        const req = store.get(entityName + '_lastSyncedAt');
        req.onsuccess = () => resolve(req.result?.value || null);
        req.onerror = e => reject(e.target.error);
    });
}

export async function setLastSyncedAt(entityName, value) {
    const db = await openDb();
    return new Promise((resolve, reject) => {
        const tx = db.transaction('UpdateDates', 'readwrite');
        const store = tx.objectStore('UpdateDates');
        const req = store.put({ name: entityName + '_lastSyncedAt', value });
        req.onsuccess = () => resolve();
        req.onerror = e => reject(e.target.error);
    });
}

export async function clearLastSyncedAt(entityName) {
    const db = await openDb();
    return new Promise((resolve, reject) => {
        const tx = db.transaction('UpdateDates', 'readwrite');
        const store = tx.objectStore('UpdateDates');
        const req = store.delete(entityName + '_lastSyncedAt');
        req.onsuccess = () => resolve();
        req.onerror = e => reject(e.target.error);
    });
}