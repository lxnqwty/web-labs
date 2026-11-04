import { ref } from 'vue'

// Единый реактивный источник данных, полученных через API-модуль.
export const items = ref([])

export function setItems(serverItems) {
  items.value = Array.isArray(serverItems) ? serverItems : []
}

export function addItem(serverItem) {
  // id и number уже назначены сервером (или mock) и не генерируются страницей.
  items.value.unshift(serverItem)
}

export const categories = [
  { id: 1, name: 'Учебная' },
  { id: 2, name: 'Научная' },
  { id: 3, name: 'Проектная' },
]

export const statusLabels = {
  New: 'Новая',
  InProgress: 'На проверке',
  Closed: 'Подтверждена',
  Cancelled: 'Отклонена',
}

export const notice = ref('')
