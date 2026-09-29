<script setup lang="ts">
import { computed } from 'vue'
import { BRANCH, REPO_URL } from '../config'

// Path relative to the repository root, e.g. "labs/01-intro/code/HelloWorld".
const props = defineProps<{ path: string }>()

const href = computed(() => `${REPO_URL}/tree/${BRANCH}/${props.path}`)
</script>

<template>
  <a :href="href" target="_blank" class="code-link">
    <span class="i-carbon-logo-github" /> {{ path }}
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
