<script setup>
import { categories, statusLabels } from '../ticketsStore.js'
defineProps({ items: { type: Array, required: true } })
function categoryName(id) {
  return categories.find((category) => category.id === Number(id))?.name ?? `Категория ${id}`
}
</script>
<template>
  <div class="table-wrap"><table>
    <thead><tr><th>НОМЕР</th><th>ТЕМА АКТИВНОСТИ</th><th>КАТЕГОРИЯ</th><th>СТАТУС</th></tr></thead>
    <tbody>
      <tr v-for="item in items" :key="item.id">
        <td class="number">#{{ item.number }}</td>
        <td><strong class="activity-name">{{ item.title }}</strong><small class="subline">Запись в цифровом профиле</small></td>
        <td><span class="category">{{ categoryName(item.categoryId) }}</span></td>
        <td><span :class="['status', 'status-' + item.status.toLowerCase()]"><i></i>{{ statusLabels[item.status] }}</span></td>
      </tr>
      <tr v-if="items.length === 0"><td colspan="4" class="empty">Ничего не найдено. Измените запрос или фильтр.</td></tr>
    </tbody>
  </table></div>
</template>
