<script>
	import PortCard from './lib/PortCard.svelte'
	import AddPortCard from './lib/AddPortCard.svelte'
  import { onMount } from 'svelte';

  let items = [];
  let showPopup = false;

  const API = 'http://localhost:5000';

  async function fetchItems() {
    const res = await fetch(API + "/board");
    items = await res.json();
  }

  onMount(fetchItems);
</script>


<h2>Asset Pulse</h2>

<div class="grid">
  {#each items as item}
		<PortCard portNumber={item.id} powerSocket={item.power_socket} ipAddress={item.ip_address} other={item.other} status={item.port_status}/>
  {/each}
	<AddPortCard />
</div>
<style>
 .grid {
    display: flex;
    flex-direction: row;
    justify-content: flex-start;
    gap: 10px;
    flex-wrap: wrap;
  }
</style>
