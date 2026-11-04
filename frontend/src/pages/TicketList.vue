<script setup>
import { computed, onMounted, ref } from 'vue'
import TicketTable from '../components/TicketTable.vue'
import { getTickets } from '../api/tickets.js'
import { items, setItems, statusLabels } from '../ticketsStore.js'

const filter = ref('Все')
const search = ref('')
const screen = ref('loading')
const loadError = ref('')
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

async function loadTickets() {
  screen.value = 'loading'
  loadError.value = ''

  try {
    const result = await getTickets()
    setItems(result)
    screen.value = result.length === 0 ? 'empty' : 'data'
  } catch (error) {
    setItems([])
    screen.value = 'error'
    loadError.value = error.status
      ? `Не удалось загрузить список (HTTP ${error.status}). Попробуйте ещё раз.`
      : 'Нет связи с сервером. Проверьте подключение и повторите попытку.'
  }
}

onMounted(loadTickets)
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
      <div class="panel-head"><div><h2 id="activity-title">История активностей</h2><p>Все ваши учебные и внеучебные результаты</p></div><label v-if="screen === 'data'" class="search">⌕ <input v-model="search" type="search" placeholder="Поиск по номеру или теме" aria-label="Поиск по номеру или теме"></label></div>

      <div v-if="screen === 'loading'" class="api-state" role="status" aria-live="polite">
        <span class="state-spinner" aria-hidden="true"></span>
        <strong>Загрузка активностей…</strong>
        <p>Получаем данные из API.</p>
      </div>
      <div v-else-if="screen === 'empty'" class="api-state" role="status">
        <strong>Записей пока нет</strong>
        <p>Добавьте первую активность в свой профиль.</p>
        <router-link class="primary action-link" to="/new">Добавить активность</router-link>
      </div>
      <template v-else-if="screen === 'data'">
        <div class="filters"><button v-for="item in filters" :key="item" type="button" :class="{ selected: filter === item }" @click="filter = item">{{ item === 'Все' ? 'Все записи' : statusLabels[item] }}</button></div>
        <TicketTable :items="visibleActivities" />
        <div class="panel-foot">Показано записей: <strong>{{ visibleActivities.length }}</strong><span>Данные получены через API-модуль</span></div>
      </template>
      <div v-else class="api-state api-state-error" role="alert">
        <strong>Не удалось загрузить активности</strong>
        <p>{{ loadError }}</p>
        <button class="primary" type="button" @click="loadTickets">Повторить загрузку</button>
      </div>
    </section>
    <p class="footnote">ⓘ Источник данных переключается в модуле src/api/tickets.js: mock или HTTP API.</p>
  </section>
</template>
