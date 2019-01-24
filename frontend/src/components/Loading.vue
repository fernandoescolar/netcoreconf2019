<template>
    <transition name="fade">
        <div v-if="loadingVisible" class="loading-container">
            <div class="in">
                <div class="clock"></div> 
                <div class="text"></div>
            </div>
        </div>
    </transition>
</template>

<script lang="ts">
import { Vue, Component, MapGetter } from 'types-vue';

@Component
export default class Loading extends Vue {
    @MapGetter({ namespace: 'app' })
    public loadingVisible!: boolean;
}

</script>

<style lang="scss">
@keyframes loading-clock {
  100% { transform: rotate(360deg); }
}

@keyframes loading-clockb {
  100% { transform: rotate(360deg); }
}

@keyframes loading-text {
  0% {
    content: "Loading";
  }
  25% {
    content: "Loading.";
  }
  50% {
    content: "Loading..";
  }
  75% {
    content: "Loading...";
  }
}

.fade-enter-active,
.fade-leave-active {
  transition: opacity .5s
}

.fade-enter,
.fade-leave-to {
  opacity: 0
}


.loading-container {
    position: fixed;
    z-index: 998;
    overflow: hidden;
    margin: auto;
    top: 0;
    left: 0;
    bottom: 0;
    right: 0;
    

    &:after {
        content: "";
        background-color: white;
        opacity: 0.9;
        position: fixed;
        top: 0;
        left: 0;
        bottom: 0;
        right: 0;
        overflow: hidden;
    }

    .in {
        z-index: 999;
        width: 100px;
        height: 150px;
        position: absolute;
        top: 50%;
        left: 50%;
        margin-left: -50px;
        margin-top: -75px;

        .clock {
            width: 80px;
            height: 80px;
            border-radius: 100%;
            background: #333;
            margin: 10px;
            border: 5px solid  #fff ;
            position: relative;
            box-shadow: 0 5px 0 0 rgba(0, 0, 0, .3),
                        inset 0 5px 0 0 rgba(0, 0, 0, .3),
                        inset 0 0 0 29px #c33;

            &:after {
                content: "";
                width: 5px;
                height: 60px;
                display:block;
                position:absolute;
                top:0;
                bottom: 0;
                left: 0;
                right: 0;
                margin: auto;
                box-shadow: inset 0px 2px 0 0 #fff, 
                            inset 0px 30px 0 0 #333;
                animation:loading-clock 1s linear infinite;
            }

            &:before {
                content:"";
                width: 5px;
                height: 50px;
                display:block;
                position:absolute;
                top:0;
                bottom: 0;
                left: 0;
                right: 0;
                margin: auto;
                box-shadow: inset 0px 2px 0 0 #fff, 
                            inset 0px 25px 0 0 #333;
                animation:loading-clockb 12s linear infinite;
            }
        }

        .text {
            width: 4rem;
            margin: auto;

            &:after {
                content: "Loading";
                font-weight: bold;
                animation-name: loading-text;
                animation-duration: 3s;
                animation-iteration-count: infinite;
            }
        }
    }
}
</style>