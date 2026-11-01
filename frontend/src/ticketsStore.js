import { ref } from 'vue'

// Единственный общий источник демонстрационных данных для списка и формы.
export const items = ref([
  { id: 1, number: 1001, title: 'Участие в научной конференции', description: 'Подготовлен и представлен доклад по результатам учебного исследования.', categoryId: 2, status: 'Closed' },
  { id: 2, number: 1002, title: 'Разработка командного веб-проекта', description: 'Создание прототипа цифрового сервиса в рамках проектной деятельности.', categoryId: 3, status: 'InProgress' },
  { id: 3, number: 1003, title: 'Успешное завершение учебного модуля', description: 'Выполнены практические задания по профильной дисциплине.', categoryId: 1, status: 'New' },
])
export const categories = [
  { id: 1, name: 'Учебная' }, { id: 2, name: 'Научная' }, { id: 3, name: 'Проектная' },
]
export const statusLabels = { New: 'Новая', InProgress: 'На проверке', Closed: 'Подтверждена', Cancelled: 'Отклонена' }
export const notice = ref('')
