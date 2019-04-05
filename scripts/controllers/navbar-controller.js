angular.module('app').controller('navbar-ctrl', 
	function ($scope, $timeout, $window, $filter, $route, $routeParams, $location, $rootScope){
	$scope.tabs = [{
		name: "Home",
		destination: "/",
	},{
		name: "Locations",
		destination: "/locations",
	},{
		name: "Characters",
		destination: "/characters",
	},{
		name: "Species",
		destination: "/species",
	},{
		name: "Civilizations",
		destination: "/civilizations",
	},{
		name: "Technologies",
		destination: "/technologies",
	},{
		name: "Mechanics",
		destination: "/mechanics",
	}];

	$scope.currentTabIndex = 0;

	$scope.navigate = function(index){
		var tab = $scope.tabs[index];
		console.log(tab);
		if(!tab.disabled){
			$scope.currentTabIndex = index;
			$location.path(tab.destination);
		}
	}
});
