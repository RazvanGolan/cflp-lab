<script setup lang="ts">
import { computed } from 'vue'
import { APP_REPO_URL } from '../config'

// tag: the code at the end of a week, e.g. "week-05".
// compare: an earlier tag, to link to the changes between the two instead.
const props = defineProps<{ tag: string, compare?: string }>()

const href = computed(() => props.compare
  ? `${APP_REPO_URL}/compare/${props.compare}...${props.tag}`
  : `${APP_REPO_URL}/tree/${props.tag}`)

const label = computed(() => props.compare
  ? `study-rooms: ${props.compare}...${props.tag}`
  : `study-rooms @ ${props.tag}`)
</script>

<template>
  <a :href="href" target="_blank" class="code-link">
    <span class="i-carbon-logo-github" /> {{ label }}
  </a>
</template>

<style scoped>
.code-link {
  display: inline-flex;
  align-items: center;
  gap: 0.4em;
  font-size: 0.8em;
  color: var(--cflp-muted);
  text-decoration: none;
  border-bottom: 1px dashed currentColor;
}
</style>
