import axios, { AxiosStatic } from "axios"
axios.defaults.baseURL="http://localhost:5171/"
export const $http:AxiosStatic = axios;