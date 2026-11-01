<script setup>
import { computed, ref } from 'vue'
import TicketTable from '../components/TicketTable.vue'
import { items, statusLabels } from '../ticketsStore.js'
const filter = ref('Все')
const search = ref('')
const filters = ['Все', 'New', 'InProgress', 'Closed', 'Cancelled']
const visibleActivities = computed(() => {
  const query = search.value.trim().toLocaleLowerCase('ru')
  return items.value.filter(item =>
    (filter.value === 'Все' || item.status === filter.value) &&
    (!query || `${item.number} ${item.title}`.toLocaleLowerCase('ru').includes(query))
  )
})
const total = computed(() => items.value.length)
const review = computed(() => items.value.filter(item => item.status === 'InProgress').length)
const approved = computed(() => items.value.filter(item => item.status === 'Closed').length)
</script>
<template>
  <section>
    <div class="heading"><div><small>ВАШ ЦИФРОВОЙ ПРОФИЛЬ</small><h1>Мои активности</h1><p>Отслеживайте свои достижения и результаты за время обучения.</p></div><router-link class="primary action-link" to="/new">＋ Добавить активность</router-link></div>
    <div class="stats">
      <article><span>Всего записей</span><strong>{{ total }}</strong><small>В вашем профиле</small></article>
      <article><span>На проверке</span><strong>{{ review }}</strong><small>Ожидают подтверждения</small></article>
      <article><span>Подтверждено</span><strong>{{ approved }}</strong><small>Успешные достижения</small></article>
    </div>
    <section class="panel" aria-labelledby="activity-title">
      <div class="panel-head"><div><h2 id="activity-title">История активностей</h2><p>Все ваши учебные и внеучебные результаты</p></div><label class="search">⌕ <input v-model="search" type="search" placeholder="Поиск по номеру или теме" aria-label="Поиск по номеру или теме"></label></div>
      <div class="filters"><button v-for="item in filters" :key="item" type="button" :class="{ selected: filter === item }" @click="filter = item">{{ item === 'Все' ? 'Все записи' : statusLabels[item] }}</button></div>
      <TicketTable :items="visibleActivities" />
      <div class="panel-foot">Показано записей: <strong>{{ visibleActivities.length }}</strong><span>Демонстрационные данные</span></div>
    </section>
    <p class="footnote">ⓘ Сейчас отображаются тестовые записи. Сохранение на сервер будет добавлено на следующем этапе.</p>
  </section>
</template>
