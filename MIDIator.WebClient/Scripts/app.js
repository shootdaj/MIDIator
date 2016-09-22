app = angular.module('MIDIatorApp', []);

function managerController($scope, $http) {
	$scope.formData = {};

	$scope.getDevices();

	$scope.getDevices = function () {
		$http.get('http://localhost:9000/midimanager/AvailableDevices')
			.success(function (data) {
				$scope.availableDevices = data;
				console.log(data);
			})
			.error(function (data) {
				console.log('Error: ' + data);
			});
	};	
}