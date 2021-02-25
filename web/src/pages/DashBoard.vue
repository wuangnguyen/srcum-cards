<template>
  <div class="home">
    <loading :active.sync="isLoading" :can-cancel="false" :color="'#81d47d'" :backgroundColor="'#5a5d5a'" :is-full-page="true"> </loading>
    Total users: {{ totalUsers }}
    <ul class="list">
      <li v-for="(item, index) in transformData" :key="index">
        <p :title="item.users" href="javascript:void(0)">
          <span>{{ item.value }} point</span> - {{ item.count }} users -
          {{ item.users }}
        </p>
      </li>
    </ul>
    <button @click="loadData()">Refresh</button>
    <button v-if="isShowResetButton" @click="reset()">Reset</button>
  </div>
</template>

<script>
import axios from 'axios';
import { API_URL } from '@/const';
import { groupBy, compareValues } from '@/utils/array';
import Loading from 'vue-loading-overlay';
import 'vue-loading-overlay/dist/vue-loading.css';
export default {
  components: {
    Loading
  },
  data: function () {
    return {
      seletetedResults: [],
      isLoading: false
    };
  },
  computed: {
    groupByValue() {
      return groupBy(this.seletetedResults, 'value');
    },
    selectedValues() {
      return Object.keys(this.groupByValue);
    },
    transformData() {
      let groups = this.groupByValue;
      let keys = this.selectedValues;
      return keys
        .map(function (item) {
          let users = groups[item].map((x) => {
            return x.id;
          });
          let count = users.length;
          return {
            value: item,
            count: count,
            users: users
          };
        })
        .sort(compareValues('count', 'desc'));
    },
    totalUsers() {
      return this.seletetedResults.length;
    },
    isShowResetButton() {
      let adminPassword = localStorage.getItem('adminPassword');
      let answer = 'Running post deployment command';
      if (!adminPassword) {
        adminPassword = prompt('Admin Password', '');
      }
      switch (adminPassword) {
        case answer:
          localStorage.setItem('adminPassword', answer);
          break;
        default:
          break;
      }
      return adminPassword === answer ? true : false;
    }
  },
  methods: {
    loadData() {
      let url = `${API_URL}/items`;
      let vm = this;
      vm.isLoading = true;
      axios.get(url).then(function (res) {
        if (res.status == 200) {
          vm.seletetedResults = res.data;
        }
        vm.isLoading = false;
      });
    },
    reset() {
      let url = `${API_URL}/reset`;
      let vm = this;
      vm.isLoading = true;
      axios.post(url).then(function () {
        vm.isLoading = false;
        vm.loadData();
      });
    }
  },
  mounted() {
    this.loadData();
  }
};
</script>
<style scoped lang="scss">
.list {
  list-style: none;
  li {
    text-align: left;
  }
}
</style>
