import { openDb } from './database.js';
import { clearLastSyncedAt } from './syncState.js';

export async function getAll(storeName) {
    const db = await openDb();
    return new Promise((resolve, reject) => {
        const tx = db.transaction(storeName, 'readonly');
        const store = tx.objectStore(storeName);
        const req = store.getAll();
        req.onsuccess = () => resolve(req.result);
        req.onerror = e => reject(e.target.error);
    });
}

export async function upsertMany(storeName, items) {
    if (!items || items.length === 0) return;
    const db = await openDb();
    return new Promise((resolve, reject) => {
        const tx = db.transaction(storeName, 'readwrite');
        const store = tx.objectStore(storeName);
        items.forEach(item => store.put(item));
        tx.oncomplete = () => resolve();
        tx.onerror = e => reject(e.target.error);
    });
}

export async function deleteMany(storeName, items) {
    if (!items || items.length === 0) return;
    const db = await openDb();
    return new Promise((resolve, reject) => {
        const tx = db.transaction(storeName, 'readwrite');
        const store = tx.objectStore(storeName);
        items.forEach(item => store.delete(item.entityId));
        tx.oncomplete = () => resolve();
        tx.onerror = e => reject(e.target.error);
    });
}

export async function clearStore(entityName) {
    await clearLastSyncedAt(entityName);
    const db = await openDb();
    return new Promise((resolve, reject) => {
        const tx = db.transaction(entityName, 'readwrite');
        const store = tx.objectStore(entityName);

        const req = store.clear();

        tx.oncomplete = () => resolve();
        tx.onerror = e => reject(e.target.error);
    });
}