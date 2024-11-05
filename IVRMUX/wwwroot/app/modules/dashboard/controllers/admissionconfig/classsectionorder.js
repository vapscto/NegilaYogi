
(function () {
    'use strict';
    angular
.module('app')
.controller('ClasssectionorderController', ClasssectionorderController)

    ClasssectionorderController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams']
    function ClasssectionorderController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams) {
        $scope.editEmployee = {};
        //var el = document.getElementById('single');
        //var sortable = Sortable.create(el);


        $scope.BindData = function () {
            
            var pageid = 2;
            apiService.getURI("Classsectionorder/getdetails", pageid).
            then(function (promosie) {
                if (promosie != null) {
                    $scope.newuser2 = promosie.classdetails
                    $scope.newuser3 = promosie.sectiondetails;
                }
                else {
                    swal("No Records Found");
                }
            })
        }

        $scope.resetLists = function () {
            $scope.configA = {
                // Changed sorting within list
                onUpdate: function (evt) {
                    var itemEl = evt.item;  // dragged HTMLElement
                    // + indexes from onEnd
                }
            };
            $scope.configB = {
                // Changed sorting within list
                onUpdate: function (evt) {
                    var itemEl = evt.item;  // dragged HTMLElement
                    // + indexes from onEnd
                }
            };
        };
        $scope.init = function () {

            $scope.resetLists();

        };
        $scope.init();
        //var e2 = document.getElementById('single1');
        //var sortable = Sortable.create(e2);
        // Time picker starts
        //$scope.timedis = true;
        //$scope.ScheduleTime = new Date();
        $scope.save = function (arrrayclass, arraysection) {
            
            alert("sfskjdfks");
        }


        $scope.hstep = 1;
        $scope.mstep = 1;

        $scope.options = {
            hstep: [1, 2, 3],
            mstep: [1, 5, 10, 15, 25, 30]
        };

        $scope.ismeridian = true;
        $scope.toggleMode = function () {
            $scope.ismeridian = !$scope.ismeridian;
        };

        // Time picker end here

        //Ui Grid view rendering data from data base



        //saving the data after rearrang the order
        $scope.save = function (newuser2, newuser3) {
            var data = {
                "classorder": $scope.newuser2,
                "secorder": $scope.newuser3,
                "flagclass": 'class',
                "flagsec": 'section'
            }
            apiService.create("Classsectionorder/save/", data).then(
                function (promoise) {
                    if (promoise != null)
                    {
                        if (promoise.retruval == true)
                        {
                            swal("Records Updated Sucessfully");
                        }
                        else
                        {
                            swal("Failed to Update the Record");
                        }
                    }
                    else
                    {
                        swal("No Records Updated");
                    }
                    $scope.BindData();
                })
        }
    }

})();