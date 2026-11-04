import { ref } from 'vue'

// Единственный общий источник демонстрационных данных для списка и формы.
export const items = ref([
  { id: 1, number: 1001, title: 'Участие в научной конференции', description: 'Подготовлен и представлен доклад по результатам учебного исследования.', categoryId: 2, status: 'Closed' },
  { id: 2, number: 1002, title: 'Разработка командного веб-проекта', description: 'Создание прототипа цифрового сервиса в рамках проектной деятельности.', categoryId: 3, status: 'InProgress' },
  { id: 3, number: 1003, title: 'Успешное завершение учебного модуля', description: 'Выполнены практические задания по профильной дисциплине.', categoryId: 1, status: 'New' },
])

// Учебный счётчик действует только в текущей сессии браузера.
let nextId = Math.max(0, ...items.value.map((item) => Number(item.id) || 0)) + 1

export function addItem({ title, categoryId, description }) {
  const id = nextId++
  const item = {
    id,
    number: `TMP-${id}`,
    title,
    categoryId,
    description,
    status: 'New',
  }
  items.value.unshift(item)
  return item
}

export const categories = [
  { id: 1, name: 'Учебная' }, { id: 2, name: 'Научная' }, { id: 3, name: 'Проектная' },
]
export const statusLabels = { New: 'Новая', InProgress: 'На проверке', Closed: 'Подтверждена', Cancelled: 'Отклонена' }
export const notice = ref('')
