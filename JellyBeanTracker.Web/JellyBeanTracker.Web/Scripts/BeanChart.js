var JellyBeanTrackerApp = JellyBeanTrackerApp || {};

$(function () {

	if (typeof(isNative) === "undefined") {
	    $.getJSON('/JellyBeans/JellyBeanGraphData', {}, function (data) {
	        JellyBeanTrackerApp.buildChart(data);
	    });
	} else {
		//todo native function
	}

    $('#editVisible').click(function() {
    	Native('EditVisible', JellyBeanTrackerApp.currentChartData);
    });

    $('#getGraphData').click(function() {
    	NativeFunc('GetGraphData', null, function (returnData) {
    		alert('Native Function was called reloading new data');
    		JellyBeanTrackerApp.buildChart(returnData);
    	});
    });
});

JellyBeanTrackerApp.buildChartStr = function (seriesData) {
    JellyBeanTrackerApp.buildChart($.parseJSON(seriesData));
};

JellyBeanTrackerApp.buildChart = function (seriesData) {
    JellyBeanTrackerApp.currentChartData = seriesData;
    $('#container').highcharts({
        title: {
            text: 'Jelly Bean Prices',
            x: -20 //center
        },
        subtitle: {
            text: 'Source: michaelridland.com',
            x: -20
        },
        xAxis: {
            categories: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun',
                'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec']
        },
        yAxis: {
            title: {
                text: '$USD'
            },
            plotLines: [{
                value: 0,
                width: 1,
                color: '#808080'
            }]
        },
        tooltip: {
            valueSuffix: '$'
        },
        legend: {
            layout: 'vertical',
            align: 'right',
            verticalAlign: 'middle',
            borderWidth: 0
        },
        series: seriesData
    });
};
