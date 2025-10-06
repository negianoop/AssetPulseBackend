<script>
  let showModal = false;

  // Form fields
  let port_number;
  let ip_address = '';
  let port_status = true;
  let power_socket;
  let other = ''; 
  async function submitForm() {
    if (!port_number || !ip_address) return;

    const payload = {
      "id": Number(port_number),
      "port_number": Number(port_number),
      "power_socket" : Number(power_socket),
      "ip_address" : ip_address,
      "port_status" : port_status === 'true',
      "other" : other,
    };

    try {
      const response = await fetch('http://localhost:5000/board', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(payload)
      });

      if (!response.ok) {
        console.error('Failed to submit form:', await response.text());
        return;
      }

      // Reset form
      port_number = '';
      ip_address = '';
      port_status = true;
      power_socket = '';
      showModal = false;
    } catch (error) {
      console.error('Error submitting form:', error);
    }
  }
</script>

<!-- Compact "+" Card -->
<div class="add-card" on:click={() => showModal = true}>+</div>

<!-- Modal Form -->
{#if showModal}
  <div class="modal-backdrop" on:click={() => showModal = false}>
    <div class="modal-form" on:click|stopPropagation>
      <div class="close" on:click={() => showModal = false}>X</div>
      <h2>Add Port</h2>
      <form on:submit|preventDefault={submitForm}>
        <input bind:value={port_number} placeholder="Port Number" required />
        <input bind:value={ip_address} placeholder="IP Address" required />
        <select bind:value={port_status} >
          <option value='true'>Online</option>
          <option value='false'>Offline</option>
        </select>
        <input bind:value={other} placeholder="other">
        <input bind:value={power_socket} placeholder="Power Socket" />
        <button type="submit">Add Port</button>
      </form>
    </div>
  </div>
{/if}

<style>
  .add-card {
    width: 100px;
    height: 96px;
    background: #e8f0fe;
    border: 2px dashed #90caf9;
    border-radius: 10px;
    font-size: 2rem;
    color: #1976d2;
    display: flex;
    align-items: center;
    justify-content: center;
    cursor: pointer;
    transition: background 0.2s;
  }

  .add-card:hover {
    background: #d0e3ff;
  }

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

  .modal-form {
    background: white;
    padding: 25px;
    border-radius: 15px;
    width: 320px;
    box-shadow: 0 8px 30px rgba(0,0,0,0.2);
    position: relative;
  }

  .modal-form .close {
    position: absolute;
    top: 18px;
    right: 16px;
    font-size: 1.2rem;
    cursor: pointer;
  }

  .modal-form h2 {
    margin-top: 0;
  }

  form {
    display: flex;
    flex-direction: column;
    gap: 12px;
  }

  input, select {
    padding: 8px;
    border-radius: 6px;
    border: 1px solid #ccc;
    font-size: 0.9rem;
  }

  button {
    margin-top: 10px;
    padding: 10px;
    background: #1976d2;
    color: white;
    border: none;
    border-radius: 6px;
    cursor: pointer;
  }

  button:hover {
    background: #1565c0;
  }
</style>


