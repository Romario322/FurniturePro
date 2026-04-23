export const DB_NAME = 'schedule-cache';
export const DB_VERSION = 1;
export const STORES = ['Categories', 'Colors', 'Materials', 'Statuses',
    'Clients', 'Counts', 'Furniture', 'Orders', 'Parts', 'Prices',
    'FurnitureCompositions', 'OrderCompositions', 'StatusChanges', 'UpdateDates'];

export function openDb() {
    return new Promise((resolve, reject) => {
        const request = indexedDB.open(DB_NAME, DB_VERSION);

        request.onupgradeneeded = event => {
            const db = event.target.result;

            if (!db.objectStoreNames.contains('Categories')) db.createObjectStore('Categories', { keyPath: 'id' });
            if (!db.objectStoreNames.contains('Colors')) db.createObjectStore('Colors', { keyPath: 'id' });
            if (!db.objectStoreNames.contains('Materials')) db.createObjectStore('Materials', { keyPath: 'id' });
            if (!db.objectStoreNames.contains('Statuses')) db.createObjectStore('Statuses', { keyPath: 'id' });
            if (!db.objectStoreNames.contains('OperationTypes')) db.createObjectStore('OperationTypes', { keyPath: 'id' });
            if (!db.objectStoreNames.contains('Clients')) db.createObjectStore('Clients', { keyPath: 'id' });

            if (!db.objectStoreNames.contains('Furniture')) db.createObjectStore('Furniture', { keyPath: 'id' });
            if (!db.objectStoreNames.contains('Orders')) db.createObjectStore('Orders', { keyPath: 'id' });
            if (!db.objectStoreNames.contains('Parts')) db.createObjectStore('Parts', { keyPath: 'id' });

            if (!db.objectStoreNames.contains('Prices')) db.createObjectStore('Prices', { keyPath: 'id' });
            if (!db.objectStoreNames.contains('Snapshots')) db.createObjectStore('Snapshots', { keyPath: 'id' });
            if (!db.objectStoreNames.contains('Operations')) db.createObjectStore('Operations', { keyPath: 'id' });

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