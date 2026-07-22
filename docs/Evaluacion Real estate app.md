# Necesito que me ayudes a construir un archivo .pdf que funcionará como una Skill para Claude Opus 4.8.



### Objetivo

El propósito de esta Skill no es evaluar un proyecto de software de manera general, sino crear una metodología de evaluación extremadamente rigurosa, equivalente al nivel de revisión de un arquitecto de software o un docente senior con más de 23 años de experiencia.

Esta Skill será utilizada junto con varios archivos PDF ubicados en la carpeta recursos. Dichos PDFs contendrán las rúbricas, instrucciones, requerimientos funcionales y no funcionales, criterios de evaluación y demás documentación oficial del proyecto.

La Skill no debe incluir el contenido de esos PDFs, ya que Claude los recibirá directamente como contexto. Su función consiste únicamente en indicar cómo debe analizarlos y cómo debe ejecutar la evaluación.



### Contexto importante

El docente que evalúa estos proyectos es extremadamente estricto.

No basta con que una funcionalidad exista para obtener el puntaje máximo.

Por ejemplo:

Que un usuario pueda registrarse correctamente no implica automáticamente un 10/10 en ese criterio.
Puede perderse puntuación por pequeños detalles.
Se descuentan puntos por errores mínimos.
Incluso aspectos como ortografía, acentuación, consistencia visual, mensajes del sistema, textos literales, validaciones, comportamiento esperado y diferencias con respecto al documento oficial afectan la calificación.

En una entrega anterior, un proyecto completamente funcional y con una buena arquitectura obtuvo solamente 9/10.

Ese punto perdido no correspondía a un único error.

Fue la consecuencia de múltiples observaciones pequeñas distribuidas en la rúbrica.

Al promediar rúbricas de más de 600 o incluso más de 1000 puntos, esas pequeñas pérdidas reducen significativamente la nota final.

Por ello, esta Skill debe asumir que el evaluador buscará absolutamente cualquier diferencia entre el proyecto y la documentación.



### Nivel de rigurosidad esperado

La Skill debe obligar a Claude a trabajar con un nivel de profundidad equivalente al de una auditoría técnica profesional.

Nunca debe asumir.

Nunca debe dar por cumplido un requisito porque "parece correcto".

Debe demostrar mediante evidencia que cada requisito realmente se cumple.

Debe intentar encontrar defectos.

Debe comportarse como un auditor cuyo objetivo es descubrir la mayor cantidad posible de incumplimientos.

La filosofía general debe ser:

Si existe la posibilidad de perder puntos, la Skill debe intentar descubrir exactamente por qué.

Problema detectado con versiones anteriores

Anteriormente existía una Skill mucho más extensa que era utilizada con Claude Sonnet/Fable.

Esa Skill ejecutaba la evaluación en múltiples fases durante aproximadamente entre 35 y 45 minutos.

### Entre sus capacidades estaban:

dividir el trabajo en etapas,
almacenar hallazgos en memoria,
cruzar evidencia entre documentos,
analizar el código completo,
construir una evaluación incremental,
mantener un registro de hallazgos,
verificar funcionalidad,
revisar arquitectura,
revisar experiencia de usuario,
revisar seguridad,
revisar mensajes,
revisar buenas prácticas,
revisar cumplimiento documental,
generar recomendaciones,
ejecutar pruebas razonadas,
producir un informe final muy detallado.

La Skill estaba organizada en ejes desde la letra A hasta la letra M.

Cada eje contenía aproximadamente entre 15 y 20 verificaciones distintas.

El resultado era una evaluación extremadamente detallada.

### Problemas actuales

Las evaluaciones recientes realizadas con Opus presentan varias deficiencias:

terminan demasiado rápido (aproximadamente 13 minutos);
parecen superficiales;
asignan calificaciones demasiado optimistas;
no justifican suficientemente cada puntuación;
omiten verificaciones importantes;
no muestran evidencia suficiente;
no indican qué pruebas realizaron internamente;
no documentan escenarios probados;
no indican porcentaje de éxito o fracaso;
generan recomendaciones demasiado generales;
incluyen comentarios irrelevantes como "solo resta ejecutar la aplicación manualmente", aun cuando no corresponden.

La nueva Skill debe evitar completamente ese comportamiento.

Lo que necesito que construyas

No quiero una simple guía.

No quiero un prompt corto.

Quiero una Skill profesional escrita en Markdown.

Debe ser lo suficientemente robusta para inducir a Claude Opus 4.8 a dedicar el máximo esfuerzo posible durante toda la evaluación.

Debe aprovechar al máximo su ventana de contexto.

Debe obligarlo a:

pensar antes de responder;
realizar múltiples pasadas sobre el código;
contrastar constantemente con los PDFs;
mantener memoria de hallazgos;
detectar inconsistencias;
reevaluar sus propias conclusiones;
evitar conclusiones apresuradas;
justificar cada decisión.
Características obligatorias

### La Skill debe incluir instrucciones para que Claude:

analice absolutamente todos los documentos disponibles;
relacione requisitos entre diferentes PDFs;
detecte contradicciones;
detecte omisiones;
identifique requisitos implícitos;
revise requisitos explícitos;
revise requisitos funcionales;
revise requisitos no funcionales;
revise arquitectura;
revise seguridad;
revise experiencia de usuario;
revise consistencia visual;
revise accesibilidad cuando corresponda;
revise calidad del código;
revise buenas prácticas;
revise mantenibilidad;
revise escalabilidad;
revise patrones utilizados;
revise convenciones;
revise nomenclatura;
revise mensajes;
revise ortografía;
revise redacción;
revise consistencia de textos;
revise validaciones;
revise manejo de errores;
revise logs;
revise pruebas;
revise escenarios límite;
revise casos negativos;
revise comportamiento esperado.
Metodología

La Skill debe diseñar una metodología por fases.

No debe realizar todo en una única pasada.

Debe definir un proceso incremental donde cada fase complemente la anterior.

Cada fase debe producir evidencia que será utilizada por las siguientes fases.

Evidencia

Cada observación debe estar respaldada.

No quiero frases como:

"Esto parece correcto."

Quiero algo similar a:

requisito encontrado;
ubicación del requisito;
evidencia encontrada;
evidencia faltante;
impacto sobre la rúbrica;
puntuación sugerida;
justificación.
Calificación

La Skill debe asumir que obtener un 10 requiere cumplir absolutamente todos los detalles.

Debe penalizar cualquier desviación.

Nunca debe regalar puntuación.

Debe explicar exactamente por qué un criterio obtuvo:

10
9
8
7
etc.

### Principio de escepticismo técnico

No asumas que un requisito está cumplido porque exista una implementación aparentemente funcional.

Todo requisito comienza en estado No demostrado.

Un requisito solo puede cambiar a Cumplido cuando exista evidencia suficiente, objetiva y verificable que demuestre que satisface completamente la especificación definida en los documentos oficiales.

En ausencia de evidencia suficiente, el estado debe permanecer como No demostrado, nunca como Cumplido.

Tu responsabilidad no es validar el proyecto rápidamente, sino intentar refutar el cumplimiento de cada requisito mediante una revisión exhaustiva. Solo cuando todos los posibles motivos razonables de incumplimiento hayan sido descartados mediante evidencia podrás considerarlo completamente satisfecho.

### Presunción de no conformidad

La puntuación máxima no se presume.

Cada punto debe ganarse mediante evidencia.

La existencia de una funcionalidad no implica automáticamente el cumplimiento total del criterio asociado.

Deben verificarse todos los aspectos explícitos e implícitos del requisito, incluyendo comportamiento, validaciones, mensajes, experiencia de usuario, calidad de implementación, consistencia con la documentación y cualquier otro elemento que pueda afectar la evaluación.

### Principio de suficiencia de evidencia

La ausencia de evidencia no constituye evidencia de cumplimiento.

Si no puedes demostrar que un requisito se cumple, no debes asumir que se cumple.

### Resultado esperado

El resultado final debe ser una Skill extremadamente profesional, organizada y modular.

Debe estar diseñada para producir evaluaciones comparables con una auditoría técnica de alto nivel y maximizar la probabilidad de detectar cualquier aspecto que pueda reducir la calificación del proyecto según las rúbricas oficiales.

Nota: los recursos para construir este md se en encuentran adjuntos al project del mensaje.

