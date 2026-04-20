const DB_NAME = 'schedule-cache';
const DB_VERSION = 1;
const STORES = ['Categories', 'Colors', 'Materials', 'Statuses',
    'Clients', 'Counts', 'Furniture', 'Orders', 'Parts', 'Prices',
    'FurnitureCompositions', 'OrderCompositions', 'StatusChanges', 'UpdateDates'];

function openDb() {
    return new Promise((resolve, reject) => {
        const request = indexedDB.open(DB_NAME, DB_VERSION);

        request.onupgradeneeded = event => {
            const db = event.target.result;

            if (!db.objectStoreNames.contains('Categories')) {
                db.createObjectStore('Categories', { keyPath: 'id' });
            }
            if (!db.objectStoreNames.contains('Colors')) {
                db.createObjectStore('Colors', { keyPath: 'id' });
            }
            if (!db.objectStoreNames.contains('Materials')) {
                db.createObjectStore('Materials', { keyPath: 'id' });
            }
            if (!db.objectStoreNames.contains('Statuses')) {
                db.createObjectStore('Statuses', { keyPath: 'id' });
            }
            if (!db.objectStoreNames.contains('OperationTypes')) {
                db.createObjectStore('OperationTypes', { keyPath: 'id' });
            }
            if (!db.objectStoreNames.contains('Clients')) {
                db.createObjectStore('Clients', { keyPath: 'id' });
            }

            if (!db.objectStoreNames.contains('Furniture')) {
                db.createObjectStore('Furniture', { keyPath: 'id' });
            }
            if (!db.objectStoreNames.contains('Orders')) {
                db.createObjectStore('Orders', { keyPath: 'id' });
            }
            if (!db.objectStoreNames.contains('Parts')) {
                db.createObjectStore('Parts', { keyPath: 'id' });
            }

            if (!db.objectStoreNames.contains('Prices')) {
                db.createObjectStore('Prices', { keyPath: 'id' });
            }
            if (!db.objectStoreNames.contains('Snapshots')) {
                db.createObjectStore('Snapshots', { keyPath: 'id' });
            }
            if (!db.objectStoreNames.contains('Operations')) {
                db.createObjectStore('Operations', { keyPath: 'id' });
            }

            if (!db.objectStoreNames.contains('FurnitureCompositions')) {
                db.createObjectStore('FurnitureCompositions', { keyPath: ['idFurniture', 'idPart'] });
            }
            if (!db.objectStoreNames.contains('OrderCompositions')) {
                db.createObjectStore('OrderCompositions', { keyPath: ['idOrder', 'idFurniture'] });
            }
            if (!db.objectStoreNames.contains('StatusChanges')) {
                db.createObjectStore('StatusChanges', { keyPath: ['idOrder', 'idStatus'] });
            }

            if (!db.objectStoreNames.contains('UpdateDates')) {
                db.createObjectStore('UpdateDates', { keyPath: 'name' });
            }
        };

        request.onsuccess = event => resolve(event.target.result);
        request.onerror = event => reject(event.target.error);
    });
}

async function getAll(storeName) {
    const db = await openDb();
    return new Promise((resolve, reject) => {
        const tx = db.transaction(storeName, 'readonly');
        const store = tx.objectStore(storeName);
        const req = store.getAll();
        req.onsuccess = () => resolve(req.result);
        req.onerror = e => reject(e.target.error);
    });
}

async function upsertMany(storeName, items) {
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

async function deleteMany(storeName, items) {
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

async function getLastSyncedAt(entityName) {
    const db = await openDb();
    return new Promise((resolve, reject) => {
        const tx = db.transaction('UpdateDates', 'readonly');
        const store = tx.objectStore('UpdateDates');
        const req = store.get(entityName + '_lastSyncedAt');
        req.onsuccess = () => resolve(req.result?.value || null);
        req.onerror = e => reject(e.target.error);
    });
}

async function setLastSyncedAt(entityName, value) {
    const db = await openDb();
    return new Promise((resolve, reject) => {
        const tx = db.transaction('UpdateDates', 'readwrite');
        const store = tx.objectStore('UpdateDates');
        const req = store.put({ name: entityName + '_lastSyncedAt', value });
        req.onsuccess = () => resolve();
        req.onerror = e => reject(e.target.error);
    });
}

async function clearLastSyncedAt(entityName) {
    const db = await openDb();
    return new Promise((resolve, reject) => {
        const tx = db.transaction('UpdateDates', 'readwrite');
        const store = tx.objectStore('UpdateDates');
        const req = store.delete(entityName + '_lastSyncedAt');
        req.onsuccess = () => resolve();
        req.onerror = e => reject(e.target.error);
    });
}

async function clearStore(entityName) {
    clearLastSyncedAt(entityName);
    const db = await openDb();
    return new Promise((resolve, reject) => {
        const tx = db.transaction(entityName, 'readwrite');
        const store = tx.objectStore(entityName);

        // The clear() method removes all records from the object store
        const req = store.clear();

        tx.oncomplete = () => resolve();
        tx.onerror = e => reject(e.target.error);
    });
}