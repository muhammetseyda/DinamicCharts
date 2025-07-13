let connectionInfo = {};
let selectedObject = {};
let chartType = '';
let chartData = [];
let currentChart = null;

async function testConnection() {
    const hostInput = document.getElementById('host');
    const databaseInput = document.getElementById('dbAdi');
    const usernameInput = document.getElementById('username');
    const passwordInput = document.getElementById('password');

    if (!hostInput || !databaseInput || !usernameInput || !passwordInput) {
        alert('Form elemanları bulunamadı!');
        return;
    }

    connectionInfo = {
        host: hostInput.value,
        dbAdi: databaseInput.value,
        username: usernameInput.value,
        password: passwordInput.value
    };

    if (!connectionInfo.host || !connectionInfo.dbAdi || !connectionInfo.username || !connectionInfo.password) {
        alert('Lütfen tüm alanları doldurun!');
        return;
    }

    console.log('Connection info:', connectionInfo);

    try {
        await loadDatabaseObjects();
        setStepActive(2);
        const objectCard = document.getElementById('objectCard');
        if (objectCard) {
            objectCard.style.display = 'block';
            objectCard.scrollIntoView({ behavior: 'smooth' });
        }
    } catch (error) {
        console.error('Connection test error:', error);
        alert('Bağlantı hatası: ' + error.message);
    }
}

async function loadDatabaseObjects() {
    const viewsLoading = document.getElementById('viewsLoading');
    if (viewsLoading) viewsLoading.classList.add('show');

    try {
        const viewsResponse = await fetch('/Chart/GetViews', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(connectionInfo)
        });

        if (viewsResponse.ok) {
            const views = await viewsResponse.json();
            console.log('Views loaded:', views);
            populateObjectList('viewsList', views, 'view');
        } else {
            console.error('Views response error:', viewsResponse.status);
            populateObjectList('viewsList', [], 'view');
        }
    } catch (error) {
        console.error('Views loading error:', error);
        populateObjectList('viewsList', [], 'view');
    }
    if (viewsLoading) viewsLoading.classList.remove('show');

    const proceduresLoading = document.getElementById('proceduresLoading');
    if (proceduresLoading) proceduresLoading.classList.add('show');

    try {
        const proceduresResponse = await fetch('/Chart/GetProcedures', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(connectionInfo)
        });

        if (proceduresResponse.ok) {
            const procedures = await proceduresResponse.json();
            console.log('Procedures loaded:', procedures);
            populateObjectList('proceduresList', procedures, 'procedure');
        } else {
            console.error('Procedures response error:', proceduresResponse.status);
            populateObjectList('proceduresList', [], 'procedure');
        }
    } catch (error) {
        console.error('Procedures loading error:', error);
        populateObjectList('proceduresList', [], 'procedure');
    }
    if (proceduresLoading) proceduresLoading.classList.remove('show');

    const functionsLoading = document.getElementById('functionsLoading');
    if (functionsLoading) functionsLoading.classList.add('show');

    try {
        const functionsResponse = await fetch('/Chart/GetFunctions', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(connectionInfo)
        });

        if (functionsResponse.ok) {
            const functions = await functionsResponse.json();
            console.log('Functions loaded:', functions);
            populateObjectList('functionsList', functions, 'function');
        } else {
            console.error('Functions response error:', functionsResponse.status);
            populateObjectList('functionsList', [], 'function');
        }
    } catch (error) {
        console.error('Functions loading error:', error);
        populateObjectList('functionsList', [], 'function');
    }
    if (functionsLoading) functionsLoading.classList.remove('show');
}

function populateObjectList(containerId, objects, type) {
    const container = document.getElementById(containerId);
    if (!container) {
        console.error('Container not found:', containerId);
        return;
    }

    container.innerHTML = '';

    if (objects && Array.isArray(objects) && objects.length > 0) {
        objects.forEach(obj => {
            const item = document.createElement('div');
            item.className = 'object-item';
            const objectName = typeof obj === 'string' ? obj : (obj.name || obj.Name || obj.TABLE_NAME || obj.ROUTINE_NAME || 'Unknown');
            item.textContent = objectName;
            item.onclick = () => selectObject(objectName, type, item);
            container.appendChild(item);
        });
    } else {
        const noDataMsg = document.createElement('p');
        noDataMsg.className = 'text-muted text-center';
        noDataMsg.textContent = 'Veri bulunamadı';
        container.appendChild(noDataMsg);
    }
}

function selectObject(name, type, element) {
    if (!element) {
        console.error('Element not found for selection');
        return;
    }

    document.querySelectorAll('.object-item').forEach(item => {
        if (item && item.classList) {
            item.classList.remove('selected');
        }
    });

    if (element.classList) {
        element.classList.add('selected');
    }

    selectedObject = { name, type };
    const loadDataBtn = document.getElementById('loadDataBtn');
    if (loadDataBtn) {
        loadDataBtn.disabled = false;
    }

    console.log('Selected object:', selectedObject);
}

async function loadDataPreview() {
    if (!selectedObject.name || !selectedObject.type) {
        alert('Lütfen bir veri objesi seçin!');
        return;
    }

    try {
        const executeRequest = {
            ...connectionInfo,
            objectType: selectedObject.type,
            objectName: selectedObject.name
        };

        const response = await fetch('/Chart/ExecuteObject', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(executeRequest)
        });

        const result = await response.json();
        chartData = result;

        if (chartData && chartData.length > 0) {
            setStepActive(3);
            document.getElementById('chartTypeCard').style.display = 'block';
            document.getElementById('chartTypeCard').scrollIntoView({ behavior: 'smooth' });
        } else {
            alert('Seçilen objeden veri alınamadı!');
        }
    } catch (error) {
        alert('Veri yükleme hatası: ' + error.message);
    }
}

function selectChartType(type) {
    chartType = type;
    document.querySelectorAll('.chart-type-card').forEach(card => {
        card.classList.remove('border-primary');
    });
    event.currentTarget.classList.add('border-primary');

    setStepActive(4);
    document.getElementById('mappingCard').style.display = 'block';
    populateAxisOptions();
    document.getElementById('mappingCard').scrollIntoView({ behavior: 'smooth' });
}

function populateAxisOptions() {
    const xAxis = document.getElementById('xAxis');
    const yAxis = document.getElementById('yAxis');

    xAxis.innerHTML = '<option value="">Seçiniz...</option>';
    yAxis.innerHTML = '<option value="">Seçiniz...</option>';

    if (chartData && chartData.length > 0) {
        const columns = Object.keys(chartData[0]);
        columns.forEach(column => {
            const option1 = document.createElement('option');
            option1.value = column;
            option1.textContent = column;
            xAxis.appendChild(option1);

            const option2 = document.createElement('option');
            option2.value = column;
            option2.textContent = column;
            yAxis.appendChild(option2);
        });
    }

    document.getElementById('xAxis').addEventListener('change', checkMappingComplete);
    document.getElementById('yAxis').addEventListener('change', checkMappingComplete);
}

function checkMappingComplete() {
    const xAxis = document.getElementById('xAxis').value;
    const yAxis = document.getElementById('yAxis').value;
    document.getElementById('generateBtn').disabled = !(xAxis && yAxis);
}

function generateChart() {
    const xAxisField = document.getElementById('xAxis').value;
    const yAxisField = document.getElementById('yAxis').value;

    if (!xAxisField || !yAxisField) {
        alert('Lütfen X ve Y ekseni seçin!');
        return;
    }

    const labels = chartData.map(item => item[xAxisField]);
    const data = chartData.map(item => item[yAxisField]);

    const ctx = document.getElementById('dynamicChart').getContext('2d');

    if (currentChart) {
        currentChart.destroy();
    }

    const chartConfig = {
        type: chartType,
        data: {
            labels: labels,
            datasets: [{
                label: yAxisField,
                data: data,
                backgroundColor: chartType === 'line' ? 'rgba(102, 126, 234, 0.2)' : 'rgba(102, 126, 234, 0.8)',
                borderColor: 'rgba(102, 126, 234, 1)',
                borderWidth: 2,
                fill: chartType === 'line'
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            scales: chartType !== 'radar' ? {
                y: {
                    beginAtZero: true
                }
            } : {}
        }
    };

    currentChart = new Chart(ctx, chartConfig);

    setStepActive(5);
    document.getElementById('chartCard').style.display = 'block';
    document.getElementById('chartCard').scrollIntoView({ behavior: 'smooth' });
}

function setStepActive(stepNumber) {
    document.querySelectorAll('.step').forEach((step, index) => {
        step.classList.remove('active');
        if (index < stepNumber - 1) {
            step.classList.add('completed');
        } else {
            step.classList.remove('completed');
        }
    });
    document.getElementById(`step${stepNumber}`).classList.add('active');
}

function resetWizard() {
    location.reload();
}

document.addEventListener('DOMContentLoaded', function () {
    const chartTypeCards = document.querySelectorAll('.chart-type-card');
    chartTypeCards.forEach(card => {
        card.addEventListener('mouseenter', function () {
            this.style.transform = 'translateY(-5px)';
            this.style.boxShadow = '0 10px 25px rgba(0,0,0,0.15)';
        });
        card.addEventListener('mouseleave', function () {
            this.style.transform = 'translateY(0)';
            this.style.boxShadow = '0 2px 4px rgba(0,0,0,0.1)';
        });
    });
});

function copyToken() {
    const tokenInput = document.getElementById("tokenInput");
    tokenInput.type = "text";
    tokenInput.select();
    tokenInput.setSelectionRange(0, 99999);

    navigator.clipboard.writeText(tokenInput.value)
        .then(() => {
            alert("Token kopyalandı!");
            tokenInput.type = "password";
            window.getSelection().removeAllRanges();
        })
        .catch(err => {
            alert("Kopyalama başarısız: " + err);
        });
}