<template>
  <div class="fab" @click="$emit('click')">+</div>
</template>

<script>
export default {
  name: 'FloatingActionButton',
  data() {
    return {};
  }
};
</script>

<style>
.fab {
  width: 48px;
  height: 48px;
  background-color: red;
  border-radius: 50%;
  box-shadow: 0 6px 10px 0 #666;
  transition: all 0.1s ease-in-out;

  font-size: 24px;
  color: white;
  text-align: center;
  line-height: 48px;

  position: fixed;
  right: 25px;
  bottom: 25px;
  cursor: pointer;
}

.fab:hover {
  box-shadow: 0 6px 14px 0 #666;
  transform: scale(1.05);
}
</style>
