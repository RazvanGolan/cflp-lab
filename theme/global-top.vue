<script setup lang="ts">
import { useDarkMode } from '@slidev/client'

// Slidev saves the choice in localStorage ("slidev-color-schema"); the landing page
// uses the same key, so one choice applies to the whole site.
const { isDark, toggleDark } = useDarkMode()
</script>

<template>
  <div class="cflp-corner">
    <!-- Decks live at <site>/<lab>/<deck>/, so the landing page is two levels up. -->
    <a class="cflp-corner-btn" href="../../" title="All labs">
      <span class="icon i-carbon-home" />
    </a>
    <button
      class="cflp-corner-btn"
      :title="isDark ? 'Switch to light mode' : 'Switch to dark mode'"
      @click="toggleDark()"
    >
      <span class="icon" :class="isDark ? 'i-carbon-sun' : 'i-carbon-moon'" />
    </button>
  </div>
</template>

<style scoped>
.cflp-corner {
  position: absolute;
  top: 14px;
  right: 16px;
  z-index: 10;
  display: flex;
  gap: 2px;
  opacity: 0.35;
  transition: opacity 0.15s;
}

.cflp-corner:hover {
  opacity: 1;
}

.cflp-corner-btn {
  display: block;
  font-size: 1.1rem;
  line-height: 1;
  padding: 6px;
  border-radius: 6px;
  color: var(--cflp-muted, #5b5b66);
}

.icon {
  display: block;
  width: 1.2em;
  height: 1.2em;
}

@media print {
  .cflp-corner {
    display: none;
  }
}
</style>
