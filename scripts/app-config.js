'use strict';

var routeConfig;

app.config(function ($routeProvider){
	for (var paths in window.routes) {
		$routeProvider.when(paths, window.routes[paths]);
	}
	$routeProvider.otherwise({redirectTo: '/'});
});