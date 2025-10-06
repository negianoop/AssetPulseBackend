<script>
  export let portNumber = '03';
  export let ipAddress = '192.168.1.45';
  export let status = 'online';
  export let powerSocket = 'PS-12A';


  let showModal = false;
  $: statusClass = status ? 'online' : 'offline';
</script>

<!-- Compact View -->
<div class="compact-card" on:click={() => showModal = true}>
  <div class="status-indicator {statusClass}"></div>
  <div class="compact-header">P{portNumber}</div>
  <div class="compact-sub">{ipAddress}</div>
</div>

<!-- Modal View -->
{#if showModal}
  <div class="modal-backdrop" on:click={() => showModal = false}>
    <div class="modal-card" on:click|stopPropagation>
      <div class="close" on:click={() => showModal = false}>X</div>
      <h2>Port {portNumber}</h2>
      <div class="info-item">
        <span class="label">Status:</span>
        <span class="value {statusClass}">{status}</span>
      </div>
      <div class="info-item">
        <span class="label">IP Address:</span>
        <span class="value">{ipAddress}</span>
      </div>
      <div class="info-item">
        <span class="label">Power Socket:</span>
        <span class="value">{powerSocket}</span>
      </div>
      
    </div>
  </div>
{/if}


<style>
  /* Compact Card Styles */
  .compact-card {
    width: 100px;
    height: 80px;
    background: #f3f3f3;
    border-radius: 10px;
    box-shadow: 0 2px 6px rgba(0, 0, 0, 0.05);
    padding: 10px;
    font-family: 'Segoe UI', sans-serif;
    cursor: pointer;
    position: relative;
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: center;
    transition: transform 0.15s ease;
    
  }

  .compact-card:hover {
    transform: scale(1.03);
    background: #fafafa;
  }

  .status-indicator {
    width: 8px;
    height: 8px;
    border-radius: 50%;
    position: absolute;
    top: 8px;
    right: 8px;
  }

  .status-indicator.online {
    background-color: #4CAF50;
  }

  .status-indicator.offline {
    background-color: #f44336;
  }

  .compact-header {
    font-size: 1rem;
    font-weight: bold;
    color: #333;
  }

  .compact-sub {
    font-size: 0.7rem;
    color: #777;
    margin-top: 4px;
    text-align: center;
  }

  /* Modal styles (same as before) */
  .modal-backdrop {
    position: fixed;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    background: rgba(0,0,0,0.4);
    display: flex;
    align-items: center;
    justify-content: center;
    z-index: 1000;
  }

  .modal-card {
    background: white;
    padding: 25px;
    border-radius: 15px;
    width: 320px;
    box-shadow: 0 8px 30px rgba(0,0,0,0.2);
    position: relative;
    padding-top: 0;
  }

  .modal-card .close {
    position: absolute;
    top: 15px;
    right: 16px;
    font-size: 1.2rem;
    cursor: pointer;
  }

  .info-item {
    display: flex;
    justify-content: space-between;
    margin-top: 10px;
    border-bottom: 1px dashed #e0e0e0;
    padding-bottom: 6px;
  }

  .label {
    font-weight: 500;
    color: #555;
  }

  .value {
    color: #333;
  }

  .value.online {
    color: #4CAF50;
  }

  .value.offline {
    color: #f44336;
  }
</style>


