






;

 

export const NO_COUNTER = { code: '', name: '', systemName: '', prefix: '', inactive: false };

 

export const PAYMENT_TYPES = ['CASH', 'CARD', 'UPI', 'SPLIT'];

// Everything the Sales Invoice setup form collects before the bill screen opens.
 

export const counterFromApi = (c) => ({ code: c.code, name: c.name, systemName: c.system_name, prefix: c.prefix, inactive: c.inactive });
