'use strict';

var routeConfig;

synapseWebApp.config(['$routeProvider'], function ($routeProvider){
	for (var paths in window.routes) {
		var pathList = paths.split(";");
		angular.forEach(pathList, function (path) {
			routeConfig = window.routes[paths];
			$routeProvider.when( path, routeConfig);
		});
	}
	$routeProvider.otherwise({redirectTo: '/'});
});