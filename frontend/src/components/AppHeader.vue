<script setup>
import { items } from '../ticketsStore.js'
</script>
<template>
  <aside class="sidebar">
    <router-link class="brand" to="/">
      <span class="brand-mark">П</span><span><strong>Профиль</strong><small>цифровая среда студента</small></span>
    </router-link>
    <p class="nav-caption">РАБОЧЕЕ ПРОСТРАНСТВО</p>
    <nav aria-label="Основная навигация">
      <router-link to="/" exact-active-class="active"><span>▤</span> Мои активности <b>{{ items.length }}</b></router-link>
      <router-link to="/new" active-class="active"><span>＋</span> Добавить запись</router-link>
    </nav>
    <div class="sidebar-bottom">
      <div class="tip"><span>✦</span><strong>Ваши достижения</strong><p>Собирайте учебные, научные и проектные результаты в одном месте.</p></div>
      <router-link class="user-button" to="/login"><span class="avatar">АК</span><span><strong>Андрей Карабаев</strong><small>Студент</small></span><i>•••</i></router-link>
    </div>
  </aside>
</template>
