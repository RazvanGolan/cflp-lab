<script setup lang="ts">
import { useDarkMode } from '@slidev/client'

// Slidev saves the choice in localStorage ("slidev-color-schema"); the landing page
// uses the same key, so one choice applies to the whole site.
const { isDark, toggleDark } = useDarkMode()
</script>

<template>
  <button
    class="cflp-theme-toggle"
    :title="isDark ? 'Switch to light mode' : 'Switch to dark mode'"
    @click="toggleDark()"
  >
    <span class="icon" :class="isDark ? 'i-carbon-sun' : 'i-carbon-moon'" />
  </button>
</template>

<style scoped>
.cflp-theme-toggle {
  position: absolute;
  top: 14px;
  right: 16px;
  z-index: 10;
  font-size: 1.1rem;
  line-height: 1;
  padding: 6px;
  border-radius: 6px;
  color: var(--cflp-muted, #5b5b66);
  opacity: 0.35;
  transition: opacity 0.15s;
}

.icon {
  display: block;
  width: 1.2em;
  height: 1.2em;
}

.cflp-theme-toggle:hover {
  opacity: 1;
}

@media print {
  .cflp-theme-toggle {
    display: none;
  }
}
</style>
