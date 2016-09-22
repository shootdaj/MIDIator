//(function() {
//	'use strict';

//	var moviesServices = angular.module('managerServices', ['ngResource']);

//	moviesServices.factory('Manager',
//	[
//		'$resource',
//		function($resource) {
//			return $resource('http://localhost:9000/midimanager/AvailableDevices',
//			{},
//			{
//				query: { method: 'GET', params: {}, isArray: true }
//			});
//		}
//	]);
//})();