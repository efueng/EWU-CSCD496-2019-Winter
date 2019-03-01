import '../styles/site.scss';

import Vue from "vue";
import GiftsComponent from './components/gifts/gifts.vue';

let v = new Vue({
    el: '#gifts',
    template: `
        <div>
            <gifts-component />
        </div>`,
    components: {
        GiftsComponent
    }
});