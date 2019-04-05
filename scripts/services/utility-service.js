'use strict';

/**
 * @ngdoc overview
 * @name apiService
 * @description
 * # API Service Factory for Admin
 *
 * Defines a factory used for holding all restful services
 */

var apiService = angular.module('apiService', ['ngResource'])
.constant('baseUrl', __synapseEnv.baseUrl)
.constant('authUrl', __synapseEnv.authUrl)
.constant('clientId', __synapseEnv.clientId)
.constant('clientSecret', __synapseEnv.clientSecret)
;

/*
 * polyfill for angular.merge
 * angular_version < 1.4*/
angular.merge = (function(angular){
  if(angular.hasOwnProperty('merge'))
    return angular.merge;
  else{
    return function merge(dst){
      var slice = [].slice;
      var isArray = Array.isArray;
      function baseExtend(dst, objs, deep) {
        for (var i = 0, ii = objs.length; i < ii; ++i) {
          var obj = objs[i];
          if (!angular.isObject(obj) && !angular.isFunction(obj)) continue;
          var keys = Object.keys(obj);
          for (var j = 0, jj = keys.length; j < jj; j++) {
            var key = keys[j];
            var src = obj[key];
            if (deep && angular.isObject(src)) {
              if (!angular.isObject(dst[key])) dst[key] = isArray(src) ? [] : {};
              baseExtend(dst[key], [src], true);
            } else {
              dst[key] = src;
            }
          }
        }
        return dst;
      }
      return baseExtend(dst, slice.call(arguments, 1), true);
    };
  }
})(angular);
