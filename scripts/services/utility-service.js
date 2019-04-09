'use strict';

angular.module("app").service("utility-service", function(){

	this.sortObjectsByAttribute = function(list, attribute){
		var output;
		list.sort(function(a, b){
			x = a[attribute];
			y = b[attribute];
			return x > y;
		});
	};
	
});