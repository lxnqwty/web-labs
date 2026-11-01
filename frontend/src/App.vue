<script setup>
import { computed } from 'vue'
import { useRoute } from 'vue-router'
import AppHeader from './components/AppHeader.vue'
import { notice } from './ticketsStore.js'
const route = useRoute()
const pageTitle = computed(() => route.meta.title ?? 'Цифровой профиль')
</script>

<template>
  <div class="app-shell">
    <AppHeader />
    <main class="main-content">
      <header class="topbar">
        <div class="crumb">Рабочее пространство <span>/</span> <strong>{{ pageTitle }}</strong></div>
        <div class="mode"><i></i> Демонстрационный режим <span class="avatar">АК</span></div>
      </header>
      <div class="page">
        <div v-if="notice" class="notice" role="status"><span>✓</span><p>{{ notice }}</p><button type="button" aria-label="Закрыть" @click="notice = ''">×</button></div>
        <router-view />
      </div>
      <footer>© 2026 Цифровой профиль студента <span>Учебный проект · Лабораторная работа №4</span></footer>
    </main>
  </div>
</template>
