<template>
  <div class="autocomplete">
    <input v-model="search" type="text" @input="onChange" @keydown.down="onArrowDown" @keydown.up="onArrowUp" @keydown.enter="onEnter">
    <ul v-show="isOpen" id="autocomplete-results" class="autocomplete-results">
      <li v-if="isLoading" class="loading">
        Loading results...
      </li>
      <li
        v-for="(result, i) in results"
        v-else
        :key="i"
        :class="{ 'is-active': i === arrowCounter }"
        class="autocomplete-result"
        @click="setResult(result)"
      >
        {{ result.caption }}
      </li>
    </ul>
  </div>
</template>

<script>
export default {
  name: 'Search',
  props: {
    items: {
      type: Array,
      required: false,
      default: () => []
    }
  },
  data() {
    return {
      isOpen: false,
      results: [],
      search: '',
      isLoading: false,
      arrowCounter: 0
    };
  },
  watch: {
    items: function (val, oldValue) {
      this.results = val;
      this.isLoading = false;
    }
  },
  mounted() {
    document.addEventListener('click', this.handleClickOutside);
  },
  destroyed() {
    document.removeEventListener('click', this.handleClickOutside);
  },
  methods: {
    onChange() {
      this.$emit('input', this.search);
      this.isOpen = true;
      this.isLoading = true;
    },
    setResult(result) {
      if (!!result) {
        this.search = result.caption;
        this.isOpen = false;

        this.$emit('selected', result.item);
      }
    },
    onArrowDown(evt) {
      if (this.arrowCounter < this.results.length) {
        this.arrowCounter = this.arrowCounter + 1;
      }
    },
    onArrowUp() {
      if (this.arrowCounter > 0) {
        this.arrowCounter = this.arrowCounter - 1;
      }
    },
    onEnter() {
      this.setResult(this.results[this.arrowCounter]);
      this.arrowCounter = -1;
    },
    handleClickOutside(evt) {
      if (!this.$el.contains(evt.target)) {
        this.isOpen = false;
        this.arrowCounter = -1;
      }
    }
  }
};
</script>

<style>
.autocomplete {
  position: relative;
}

.autocomplete input {
  width: 500px;
}

.autocomplete-results {
  color: black;
  background-color: white;
  padding: 0;
  margin: 0;
  height: auto;
  overflow: auto;
  width: 100%;
}

.autocomplete-result {
  list-style: none;
  text-align: left;
  padding: 4px 2px;
  cursor: pointer;
}

.autocomplete-result.is-active,
.autocomplete-result:hover {
  background-color: #4aae9b;
  color: white;
}
</style>
