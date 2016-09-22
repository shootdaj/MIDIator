app = angular.module('MIDIatorApp', ['angular-loading-bar'])
	.config(['cfpLoadingBarProvider', function (cfpLoadingBarProvider) {
		cfpLoadingBarProvider.includeSpinner = false;
	}])
	.controller('managerController',
		function managerController($scope, $http) {

			$scope.getDevices = function () {
				$http.get('http://localhost:9000/midimanager/AvailableDevices')
					.success(function (data) {
						$scope.availableDevices = data;
						console.log(data);
					})
					.error(function (data) {
						console.log('Error: ' + data);
					});
			}

			$scope.initialize = function (e) {
				$scope.getDevices();
			};

			$scope.initialize();
		});