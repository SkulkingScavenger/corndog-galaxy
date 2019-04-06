'use strict';

window.routes = {

	"/": {
	 	title :'Neoyahazeer Home',
		templateUrl: 'partials/pages/home.html',
		controller: 'home-ctrl',
	}, 
	"/locations": {
		title :'Neoyahazeer Locations',
		templateUrl: 'partials/pages/locations.html',
		controller: 'regions-ctrl',
	},
	"/locations/:index": {
		title :'Neoyahazeer Locations',
		templateUrl: 'partials/pages/location-info.html',
		controller: 'region-info-ctrl',
	},
	"/characters": {
		title :'Neoyahazeer Regions',
		templateUrl: 'partials/pages/characters.html',
		controller: 'character-ctrl',
	},
	"/characters/:index": {
		title :'Neoyahazeer Characters',
		templateUrl: 'partials/pages/character-info.html',
		controller: 'character-info-ctrl',
	},
	"/civilizations": {
		title :'Neoyahazeer Civilizations',
		templateUrl: 'partials/pages/civilizations.html',
		controller: 'civilizations-ctrl',
	},
	"/civilizations/:index": {
		title :'Neoyahazeer Civilizations',
		templateUrl: 'partials/pages/civilization-info.html',
		controller: 'civilization-info-ctrl',
	},
	"/species": {
		title :'Neoyahazeer Species',
		templateUrl: 'partials/pages/species.html',
		controller: 'species-ctrl',
	},
	"/species/:index": {
		title :'Neoyahazeer Species',
		templateUrl: 'partials/pages/species-info.html',
		controller: 'species-info-ctrl',
	},
	"/technologies": {
		title :'Neoyahazeer Technologies',
		templateUrl: 'partials/pages/technologies.html',
		controller: 'technologies-ctrl',
	},
	"/technologies/:index": {
		title :'Neoyahazeer Technologies',
		templateUrl: 'partials/pages/technologies-info.html',
		controller: 'technologies-info-ctrl',
	},
	"/mechanics": {
		title :'Neoyahazeer Mechanics',
		templateUrl: 'partials/pages/mechanics.html',
		controller: 'mechanics-ctrl',
	},
	"/mechanics/:index": {
		title :'Neoyahazeer Mechanics',
		templateUrl: 'partials/pages/mechanics-info.html',
		controller: 'mechanics-info-ctrl',
	},
};