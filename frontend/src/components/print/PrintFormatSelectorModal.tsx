// Print dialog: pick a legacy .rpt format for the current document, preview it with the matching web
// layout from print/layouts/, then print (⌘/Ctrl+P). Uses sample data when a screen has nothing
// to print yet (samplePrintData.ts).

import { useCallback, useEffect, useState } from 'react';
import { createPortal } from 'react-dom';
import { Check, FileText, Printer, RotateCcw, X, ZoomIn, ZoomOut } from 'lucide-react';
import { api } from '../../lib/api';
import { useCompanyProfile } from '../../hooks/useCompanyProfile';
import { printButton, secondaryButton } from '../../lib/styles';
import {
	type DocumentCategory,
	type PrinterStructure,
	type RptModelDef,
	RPT_MODELS,
	getModelsForCategory,
	getSavedModelPreference,
	saveModelPreference,
} from './rptRegistry';
import { getEffectivePrintData, SAMPLE_COMPANY } from './samplePrintData';
import { A4GstTaxInvoiceLayout } from './layouts/A4GstTaxInvoiceLayout';
import { A5SlipLayout } from './layouts/A5SlipLayout';
import { AnalyticalReportLayout, type AnalyticalReportData } from './layouts/AnalyticalReportLayout';
import { DotMatrixLayout } from './layouts/DotMatrixLayout';
import { DsrAuditLayout } from './layouts/DsrAuditLayout';
import { StockBalancePrintLayout } from './layouts/StockBalancePrintLayout';
import { ThermalPosLayout } from './layouts/ThermalPosLayout';

interface PrintFormatSelectorModalProps {
	category: DocumentCategory;
	data: unknown;
	title?: string;
	onClose: () => void;
	onPrinted?: () => void;
	initialModelId?: string;
}

const PAGE_CLASSES = [
	'print-mode-thermal-80',
	'print-mode-thermal-58',
	'print-mode-a4-portrait',
	'print-mode-a4-landscape',
	'print-mode-a5',
	'print-mode-dmp-80',
	'print-mode-dmp-132',
];

export default function PrintFormatSelectorModal({
	category,
	data,
	title = 'Select print format',
	onClose,
	onPrinted,
	initialModelId,
}: PrintFormatSelectorModalProps) {
	const { company } = useCompanyProfile();
	const [models, setModels] = useState<RptModelDef[]>(() => getModelsForCategory(category));
	const [selectedModelId, setSelectedModelId] = useState(() => {
		const requested = initialModelId ? RPT_MODELS[initialModelId] : undefined;
		return requested?.category === category ? requested.id : getSavedModelPreference(category);
	});
	const [watermark, setWatermark] = useState('None');
	const [copies, setCopies] = useState(1);
	const [rememberFormat, setRememberFormat] = useState(true);
	const [zoom, setZoom] = useState(100);

	// Fetch dynamic list of RPT template files from backend /reports/rpt-files
	useEffect(() => {
		api<Array<RptModelDef & { rptFileName?: string }>>(`/reports/rpt-files?category=${category}`)
			.then(remoteList => {
				if (Array.isArray(remoteList) && remoteList.length > 0) {
					const mapped: RptModelDef[] = remoteList.map(item => {
						const local = RPT_MODELS[item.id] || RPT_MODELS[item.rptFileName?.replace('.rpt', '') || ''];
						return {
							id: item.id,
							rptFileName: item.rptFileName || `${item.id}.rpt`,
							name: local?.name || item.name,
							category: (local?.category || item.category) as DocumentCategory,
							printer: (local?.printer || item.printer) as PrinterStructure,
							paperName: local?.paperName || item.paperName,
							badge: local?.badge || item.badge,
							description: local?.description || item.description,
							supportsCopies: local?.supportsCopies ?? (item.printer !== 'THERMAL_80MM' && item.printer !== 'THERMAL_58MM'),
							supportsWatermark: local?.supportsWatermark ?? true,
						};
					});
					setModels(mapped);
				}
			})
			.catch(() => {
				setModels(getModelsForCategory(category));
			});
	}, [category]);

	const selectedModel = models.find(model => model.id === selectedModelId) ?? models[0];

	useEffect(() => {
		if (rememberFormat && selectedModel) saveModelPreference(category, selectedModel.id);
	}, [category, rememberFormat, selectedModel]);

	useEffect(() => {
		return () => {
			document.body.classList.remove(...PAGE_CLASSES);
		};
	}, []);

	const closeAfterPrint = () => {
		document.body.classList.remove(...PAGE_CLASSES);
	};

	const print = useCallback(() => {
		if (!selectedModel) return;
		const pageClass = selectedModel.printer === 'THERMAL_80MM'
			? 'print-mode-thermal-80'
			: selectedModel.printer === 'THERMAL_58MM'
				? 'print-mode-thermal-58'
				: selectedModel.printer === 'LASER_A4_LANDSCAPE'
					? 'print-mode-a4-landscape'
					: selectedModel.printer === 'LASER_A5_LANDSCAPE'
						? 'print-mode-a5'
						: selectedModel.printer === 'DOT_MATRIX_80'
							? 'print-mode-dmp-80'
							: selectedModel.printer === 'DOT_MATRIX_132'
								? 'print-mode-dmp-132'
								: 'print-mode-a4-portrait';

		document.body.classList.remove(...PAGE_CLASSES);
		document.body.classList.add(pageClass);
		window.addEventListener('afterprint', closeAfterPrint, { once: true });
		window.print();
		onPrinted?.();
	}, [onPrinted, selectedModel]);

	useEffect(() => {
		const onKeyDown = (event: KeyboardEvent) => {
			if (event.key === 'Escape') onClose();
			if ((event.metaKey || event.ctrlKey) && event.key.toLowerCase() === 'p') {
				event.preventDefault();
				print();
			}
		};
		window.addEventListener('keydown', onKeyDown);
		return () => window.removeEventListener('keydown', onKeyDown);
	}, [onClose, print]);

	const renderLayout = (isPrintPreview: boolean) => {
		if (!selectedModel) return null;
		const effectiveData = getEffectivePrintData(category, data);
		const effectiveCompany = company || SAMPLE_COMPANY;
		const props = { modelId: selectedModel.id, data: effectiveData, company: effectiveCompany, watermark, isPrintPreview };

		if (category === 'GENERIC_REPORT') return <AnalyticalReportLayout {...props} data={effectiveData as AnalyticalReportData} />;
		if (category === 'STOCK_BALANCE') return <StockBalancePrintLayout {...props} />;
		if (category === 'DSR') {
			return selectedModel.printer === 'THERMAL_80MM' || selectedModel.printer === 'THERMAL_58MM'
				? <ThermalPosLayout {...props} />
				: <DsrAuditLayout {...props} />;
		}
		if (selectedModel.printer === 'THERMAL_80MM' || selectedModel.printer === 'THERMAL_58MM') return <ThermalPosLayout {...props} />;
		if (selectedModel.printer === 'DOT_MATRIX_80' || selectedModel.printer === 'DOT_MATRIX_132') return <DotMatrixLayout {...props} />;
		if (selectedModel.printer === 'LASER_A5_LANDSCAPE') return <A5SlipLayout {...props} />;
		return <A4GstTaxInvoiceLayout {...props} />;
	};

	const selectModel = (model: RptModelDef) => {
		setSelectedModelId(model.id);
		if (!model.supportsCopies) setCopies(1);
		if (!model.supportsWatermark) setWatermark('None');
	};

	return (
		<>
			{/* Mount directly to document.body outside #root so @media print renders cleanly */}
			{createPortal(
				<div id="vlqms-print-portal" className="hidden print:block" aria-hidden="true">
					{Array.from({ length: selectedModel?.supportsCopies ? copies : 1 }, (_, index) => (
						<div key={index} className="print-copy-wrapper">{renderLayout(false)}</div>
					))}
				</div>,
				document.body
			)}

			<div className="fixed inset-0 z-50 flex items-center justify-center bg-black/60 p-3 backdrop-blur-sm print:hidden sm:p-6" onMouseDown={event => { if (event.target === event.currentTarget) onClose(); }}>
				<section role="dialog" aria-modal="true" aria-labelledby="print-modal-title" className="flex h-[92vh] w-full max-w-6xl flex-col overflow-hidden rounded-xl border border-line bg-surface text-stone-900 shadow-2xl dark:text-stone-50">
					<header className="flex items-center justify-between gap-4 border-b border-line bg-subtle/50 px-5 py-4">
						<div className="flex min-w-0 items-center gap-3">
							<span className="flex h-9 w-9 shrink-0 items-center justify-center rounded-lg bg-accent text-white"><Printer size={18} /></span>
							<div className="min-w-0">
								<h2 id="print-modal-title" className="truncate text-base font-bold">{title}</h2>
								<p className="text-xs text-stone-500 dark:text-stone-400">{models.length} formats available from backend</p>
							</div>
						</div>
						<button type="button" onClick={onClose} className="rounded-md p-2 text-stone-500 hover:bg-muted" aria-label="Close print formats"><X size={18} /></button>
					</header>

					<div className="grid min-h-0 flex-1 grid-cols-1 overflow-hidden lg:grid-cols-12">
						<div className="space-y-5 overflow-y-auto border-b border-line bg-subtle/20 p-4 lg:col-span-5 lg:border-b-0 lg:border-r lg:p-5">
							<div className="space-y-2">
								<div className="flex items-center justify-between text-[11px] font-semibold uppercase text-stone-500">
									<span>Backend .RPT Formats</span><span>{selectedModel?.paperName}</span>
								</div>
								{models.map(model => {
									const selected = model.id === selectedModel?.id;
									return <button key={model.id} type="button" onClick={() => selectModel(model)} aria-pressed={selected}
										className={`flex w-full items-start gap-3 rounded-lg border p-3 text-left transition-colors ${selected ? 'border-accent bg-accent/5 dark:bg-accent/10' : 'border-line bg-surface hover:bg-subtle'}`}>
										<span className={`mt-0.5 flex h-5 w-5 shrink-0 items-center justify-center rounded-full border ${selected ? 'border-accent bg-accent text-white' : 'border-stone-300 dark:border-stone-600'}`}>{selected && <Check size={12} />}</span>
										<span className="min-w-0 flex-1">
											<span className="flex items-start justify-between gap-2">
												<span className="text-sm font-semibold">{model.name}</span>
												<span className="shrink-0 rounded border border-line bg-muted px-2 py-0.5 text-[10px] font-semibold">{model.badge}</span>
											</span>
											<span className="mt-1 block text-xs text-stone-500 dark:text-stone-400">{model.description}</span>
											<span className="mt-2 flex flex-wrap items-center gap-2 text-[11px] text-stone-500">
												<span className="font-mono font-bold text-accent">{model.rptFileName}</span>
												<span aria-hidden="true">·</span>
												<span>{model.paperName}</span>
											</span>
										</span>
									</button>;
								})}
								{!models.length && <p className="py-6 text-center text-sm text-stone-500">Loading print formats from backend…</p>}
							</div>

							{selectedModel && <div className="space-y-4 border-t border-line pt-4">
								<h3 className="text-xs font-semibold uppercase text-stone-500">Print options</h3>
								<div className="grid grid-cols-2 gap-3">
									<label className="text-xs font-medium text-stone-600 dark:text-stone-300">
										Copies
										<select value={selectedModel.supportsCopies ? copies : 1} onChange={event => setCopies(Number(event.target.value))} disabled={!selectedModel.supportsCopies} className="mt-1.5 w-full rounded-md border border-line bg-surface px-2.5 py-2 disabled:opacity-50">
											{[1, 2, 3].map(count => <option key={count} value={count}>{count}</option>)}
										</select>
									</label>
									<label className="text-xs font-medium text-stone-600 dark:text-stone-300">
										Watermark
										<select value={watermark} onChange={event => setWatermark(event.target.value)} disabled={!selectedModel.supportsWatermark} className="mt-1.5 w-full rounded-md border border-line bg-surface px-2.5 py-2 disabled:opacity-50">
											{['None', 'DUPLICATE COPY', 'ORIGINAL FOR RECIPIENT', 'AUDIT COPY', 'CANCELLED'].map(value => <option key={value}>{value}</option>)}
										</select>
									</label>
								</div>
								<label className="flex items-center gap-2 rounded-md border border-line bg-surface p-3 text-xs">
									<input type="checkbox" checked={rememberFormat} onChange={event => setRememberFormat(event.target.checked)} className="h-4 w-4 accent-accent" />
									Remember this format for this report type
								</label>
							</div>}
						</div>

						<div className="flex min-h-0 flex-col overflow-hidden bg-[#EFECE6] dark:bg-[#121212] lg:col-span-7">
							<div className="flex items-center justify-between border-b border-line bg-surface/80 px-4 py-2 text-xs">
								<span className="flex items-center gap-2 text-stone-500"><FileText size={14} />Preview: <strong className="font-mono text-stone-800 dark:text-stone-200">{selectedModel?.rptFileName}</strong> ({selectedModel?.paperName})</span>
								<div className="flex items-center gap-1">
									<button type="button" onClick={() => setZoom(value => Math.max(60, value - 10))} className="rounded p-1.5 hover:bg-subtle" aria-label="Zoom out"><ZoomOut size={15} /></button>
									<span className="w-10 text-center font-mono text-[11px]">{zoom}%</span>
									<button type="button" onClick={() => setZoom(value => Math.min(150, value + 10))} className="rounded p-1.5 hover:bg-subtle" aria-label="Zoom in"><ZoomIn size={15} /></button>
									<button type="button" onClick={() => setZoom(100)} className="rounded p-1.5 hover:bg-subtle" aria-label="Reset zoom"><RotateCcw size={14} /></button>
								</div>
							</div>
							<div className="flex-1 overflow-auto p-4 sm:p-6">
								<div className="mx-auto w-fit origin-top transition-transform" style={{ transform: `scale(${zoom / 100})` }}>
									{renderLayout(true)}
								</div>
							</div>
						</div>
					</div>

					<footer className="flex items-center justify-end gap-2 border-t border-line bg-surface px-4 py-3 sm:px-5">
						<button type="button" onClick={onClose} className={secondaryButton}>Cancel</button>
						<button type="button" onClick={print} disabled={!selectedModel} className={printButton}><Printer size={14} />Print · {selectedModel?.rptFileName}</button>
					</footer>
				</section>
			</div>
		</>
	);
}
