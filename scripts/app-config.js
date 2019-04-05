'use strict';

var routeConfig;

app.config(function ($routeProvider){
	for (var paths in window.routes) {
		console.log(paths);
		console.log(window.routes[paths]);
		// var pathList = paths.split(";");
		// angular.forEach(pathList, function (path) {
		// 	routeConfig = window.routes[paths];
		// 	$routeProvider.when(path, routeConfig);
		// });
	}
	$routeProvider.otherwise({redirectTo: '/'});
});