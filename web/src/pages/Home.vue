<template>
  <div class="main-container">
    <loading :active.sync="isLoading" :can-cancel="false" :color="'#81d47d'" :backgroundColor="'#5a5d5a'" :is-full-page="true"> </loading>
    <swiper class="cards-container" ref="mySwiper" :options="swiperOptions">
      <swiper-slide v-for="item in cardValue" :key="item">
        <img :src="`./img/cards/${item}.jpg`" />
      </swiper-slide>
    </swiper>

    <div class="summary">
      <div class="controls">
        <button @click="onOK" class="button"><i class="ok"></i></button>
      </div>
      <hr />
      <span v-bind:style="{ visibility: isValueSelected ? 'visible' : 'hidden' }" class="left">Selected value: {{ selectedValue }}</span>
      <span class="right">Your id: {{ randomId }}</span>
    </div>
  </div>
</template>

<script>
import { Swiper, SwiperSlide } from 'vue-awesome-swiper';
import 'swiper/css';
import axios from 'axios';
import { API_URL } from '@/const';
import Loading from 'vue-loading-overlay';
import 'vue-loading-overlay/dist/vue-loading.css';
export default {
  components: {
    Swiper,
    SwiperSlide,
    Loading
  },
  data: function () {
    return {
      cardValue: [0, 1, 2, 3, 5, 8, 13],
      swiperOptions: {
        loop: true
      },
      selectedValue: -1,
      isLoading: false
    };
  },
  computed: {
    swiper() {
      return this.$refs.mySwiper.$swiper;
    },
    isValueSelected() {
      return this.selectedValue != -1;
    },
    randomId() {
      var uidItem = localStorage.getItem('uid');
      if (!uidItem) {
        uidItem = this.uuidv4();
        localStorage.setItem('uid', uidItem);
      }
      return uidItem.split('-')[0];
    }
  },
  methods: {
    onOK() {
      var vm = this;
      vm.isLoading = true;
      vm.selectedValue = vm.cardValue[vm.swiper.realIndex];
      let url = `${API_URL}/item`;
      axios.post(url, { id: `${this.randomId}`, value: `${vm.selectedValue}` }).then(function () {
        vm.isLoading = false;
      });
    },
    uuidv4() {
      return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
        var r = (Math.random() * 16) | 0,
          v = c == 'x' ? r : (r & 0x3) | 0x8;
        return v.toString(16);
      });
    }
  },
  mounted() {
    axios.defaults.headers.post['id'] = this.randomId;
  }
};
</script>

<style lang="scss">
.cards-container img {
  max-width: 104%;
  margin: -10px;
}
.controls {
  text-align: center;
  display: flex;
  display: -webkit-flex;
  justify-content: space-around;
  margin: 0 auto;
  margin-bottom: -35px;
  .button {
    border: none;
    background: none;
    position: relative;
    display: inline-block;
    cursor: pointer;
    font-size: 16px;
    width: 60px;
    height: 60px;
    z-index: 100;
    -webkit-tap-highlight-color: rgba(0, 0, 0, 0);
    border-radius: 50%;
    background: #fff;
    border: 2px solid #9a9a9a;
    &:focus {
      outline: 0;
    }
    .ok {
      display: inline-block;
      width: 10px;
      height: 5px;
      background: rgb(129, 212, 125);
      line-height: 0;
      font-size: 0;
      vertical-align: middle;
      -webkit-transform: rotate(45deg);
      left: -5px;
      top: 2px;
      position: relative;
      &:after {
        content: '/';
        display: block;
        width: 20px;
        height: 5px;
        background: rgb(129, 212, 125);
        -webkit-transform: rotate(-90deg) translateY(-50%) translateX(50%);
      }
    }
  }
}
.summary {
  position: fixed;
  bottom: 0;
  right: 0;
  left: 0;
  margin: 0 5px 10px;
  z-index: 9999;
  span {
    display: inline-block;
    &.left {
      text-align: left;
      width: 60%;
    }
    &.right {
      text-align: right;
      width: 40%;
    }
  }
}
</style>
