'use strict';

window.routes = {

	"/": {
	 	title :'Neoyahazeer Home',
		templateUrl: 'partials/hompage.html',
		controller: 'homePageCtrl',
	}, 
	"/regions": {
		title :'Neoyahazeer Regions',
		templateUrl: 'partials/regions.html',
		controller: 'regionsCtrl',
	},
	"/regions/:index": {
		title :'Neoyahazeer Regions',
		templateUrl: 'partials/regionInfo.html',
		controller: 'regionInfoCtrl',
	},
};