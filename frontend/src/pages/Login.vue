<script setup>
import { ref } from 'vue'
const login = ref({ username: '', password: '' })
const submitted = ref(false)
function submitLogin() { submitted.value = true }
</script>
<template>
  <section class="login-area">
    <div class="login-card"><div class="brand-mark">П</div><small>С РАДОСТЬЮ ВИДИМ ВАС СНОВА</small><h1>Вход в профиль</h1><p>Войдите, чтобы управлять своими активностями.</p>
      <form @submit.prevent="submitLogin">
        <div class="field"><label for="login">Логин</label><input id="login" v-model="login.username" name="login" autocomplete="username" placeholder="Введите логин" required></div>
        <div class="field"><label for="password">Пароль</label><input id="password" v-model="login.password" name="password" type="password" autocomplete="current-password" placeholder="Введите пароль" required></div>
        <button class="primary" type="submit">Войти в систему →</button>
      </form>
      <small class="login-note">{{ submitted ? 'Демонстрационный режим: серверная авторизация ещё не подключена.' : 'Демонстрационный экран · авторизация пока не подключена' }}</small>
    </div>
  </section>
</template>
